using Application.DTOs.Account;
using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities.Identity;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Helpers;
using Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly SilverwarePOSEmailSenders _emailSenderAddresses;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRandomNumberGeneratorInterface _randomNumberGenerator;
        private IHttpContextAccessor _accessor;
        //private readonly IMandrillEmailService _mandrillEmailService;
        private readonly ILogger _logger;


        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IOptions<SilverwarePOSEmailSenders> emailSenderAddresses,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IRandomNumberGeneratorInterface randomNumberGenerator,
            ApplicationDbContext context,
            IHttpContextAccessor accessor,
            //IMandrillEmailService mandrillEmailService,
            ILogger logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _emailSenderAddresses = emailSenderAddresses.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _randomNumberGenerator = randomNumberGenerator;
            _accessor = accessor;
            //_mandrillEmailService = mandrillEmailService;
            _logger = logger;
        }

        public ClaimsPrincipal User => _accessor.HttpContext.User;

        //login
        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress = null)
        {
            try
            {
                //verify user

                //var user = await _userManager.FindByEmailAsync(request.Username);
                var user = await _context.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new ApiException($"No Accounts Registered with {request.Username}.");
                }

                //very credentials
                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    throw new ApiException($"Invalid Credentials for '{request.Username}'.");
                }

                if (!user.EmailConfirmed)
                {
                    throw new ApiException($"Account Not Confirmed for '{request.Username}'.");
                }

                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                AuthenticationResponse response = new AuthenticationResponse();
                response.Id = user.Id;
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.Email = user.Email;
                response.UserName = user.UserName;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                response.Roles = rolesList.ToList();
                response.IsVerified = user.EmailConfirmed;

                //Generate Refresh Token
                var refreshToken = GenerateRefreshToken(ipAddress);

                user.RefreshToken = refreshToken.Token;
                user.RefreshTokenExpiry = refreshToken.Expires;
                user.RefreshToken = refreshToken.Token;
                _context.Update(user);
                _context.SaveChanges();

                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpiration = refreshToken.Expires;

                return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        //For employee:
        public async Task<Response<RegisterEmployeeResponse>> RegisterEmployeeAsync(RegisterEmployeeRequest request, string origin)
        {
            try
            {
                var existingUsername = await _userManager.FindByEmailAsync(request.Email);
                if (existingUsername != null)
                {
                    return new Response<RegisterEmployeeResponse>(null, message: $"Specified email is already registered.", isSuccessful: false);
                }

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    EmailConfirmed = true,     
                    PhoneNumberConfirmed = true,
                    isActive = true
                };

                
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Employee.ToString());

                    return new Response<RegisterEmployeeResponse>(null, message: $"Your account has been created successfuly. ", isSuccessful: true);
                }
                else
                {
                    throw new ApiException($"{string.Join(", ", result.Errors.Select(x => x.Description))}");
                }
      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

       
       

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            try
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                var roleClaims = new List<Claim>();

                for (int i = 0; i < roles.Count; i++)
                {
                    roleClaims.Add(new Claim("roles", roles[i]));
                }

                string ipAddress = IpHelper.GetIpAddress();

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
                }
                .Union(userClaims)
                .Union(roleClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> GetVerificationUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(userId);
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
                }
                else
                {
                    throw new ApiException($"An error occured while confirming {user.Email}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }

        }

        private RefreshToken GenerateRefreshToken(string ipAddress = null)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress == null ? null : ipAddress
            };
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.Email);

                // always return ok response to prevent email enumeration
                if (account == null) return;

                var code = await _userManager.GeneratePasswordResetTokenAsync(account);
                var route = "api/account/reset-password/";
                var _enpointUri = new Uri(string.Concat($"{origin}/", route));
                var emailRequest = new EmailRequest()
                {
                    Body = $"You reset token is - {code}",
                    To = model.Email,
                    Subject = "Reset Password",
                };
                //await _emailService.SendAsync(emailRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.Email);
                if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
                var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return new Response<string>(model.Email, message: $"Password Reset.");
                }
                else
                {
                    throw new ApiException($"Error occured while reseting the password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        // refresh refresh token 
        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token)
        {
            try
            {
                var authenticationModel = new AuthenticationResponse();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);

                if (user == null)
                {
                    throw new ApiException($"Token did not match any users");
                }

                var refreshToken = user.RefreshToken;

                if (user.RefreshTokenExpiry < DateTime.UtcNow.AddHours(1))
                {
                    throw new ApiException($"Refresh Token is Inactive. Please login again.");
                }

                //Revoke Current Refresh Token


                //Generate new Refresh Token and save to Database
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshToken = newRefreshToken.Token;
                user.RefreshTokenExpiry = newRefreshToken.Expires;

                _context.Update(user);
                _context.SaveChanges();

                //Generate new jwt
                authenticationModel.IsVerified = true;
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                authenticationModel.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                authenticationModel.RefreshToken = newRefreshToken.Token;
                authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
                return new Response<AuthenticationResponse>(authenticationModel, $"New refresh token generated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        //revoke token
        public bool RevokeToken(string token)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.RefreshToken == token);

                // return false if no user found with token
                if (user == null) return false;

                var refreshToken = user.RefreshToken;

                // return false if token is not active
                if (user.RefreshTokenExpiry < DateTime.UtcNow.AddHours(1)) return false;

                // revoke token and save
                user.RefreshTokenExpiry = DateTime.UtcNow;

                _context.Update(user);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // add-role

        //public async Task<string> AddRoleAsync(AddRoleModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);

        //    if (user == null)
        //    {
        //        return $"No Accounts Registered with {model.Email}.";
        //    }

        //    var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == model.Role.ToLower());
        //    if (roleExists)
        //    {
        //        var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
        //        await _userManager.AddToRoleAsync(user, validRole.ToString());
        //        return $"Added {model.Role} to user {model.Email}.";
        //    }
        //    return $"Role {model.Role} not found.";

        //}

        public ApplicationUser GetById(string id)
        {
            try
            {
                return _context.Users.Find(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

    }

}
