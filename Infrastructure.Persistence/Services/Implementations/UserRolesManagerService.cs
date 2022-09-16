using Application.DTOs.Account.UserRoleManagement;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities.Identity;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Models;
using Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class UserRolesManagerService : IUserRolesManagerService
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly SilverwarePOSEmailSenders _emailSenderAddresses;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRandomNumberGeneratorInterface _randomNumberGenerator;

        public UserRolesManagerService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IOptions<SilverwarePOSEmailSenders> emailSenderAddresses,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IRandomNumberGeneratorInterface randomNumberGenerator,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _emailSenderAddresses = emailSenderAddresses.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _randomNumberGenerator = randomNumberGenerator;

        }





        public async Task<Response<UserRolesViewModel>> AddUserToRoles(AddUserToRolesRequest addUserToRoleRequest)
        {
            UserRolesViewModel userRoles = new UserRolesViewModel();

            var user = await _userManager.FindByIdAsync(addUserToRoleRequest.UserId);

            if (user == null)
            {
                return new Response<UserRolesViewModel>(userRoles, message: $"User does not exist", isSuccessful: false);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return new Response<UserRolesViewModel>(userRoles, message: $"An error occurred while attempting to remove existing roles.", isSuccessful: false);
            }

            foreach (var role in _roleManager.Roles)
            {
                var AddtoRolesresult = await _userManager.AddToRolesAsync(user, addUserToRoleRequest.Roles.Where(x => x.Selected).Select(y => y.RoleName));
                if (!AddtoRolesresult.Succeeded)
                {
                    return new Response<UserRolesViewModel>(userRoles, message: $"An error occurred while attempting to remove existing roles.", isSuccessful: false);
                }
            }

            //get just added roles
            var justAddedRoles = await GetUserRoles(user);

            userRoles.FirstName = user.FirstName;
            userRoles.Email = user.Email;
            userRoles.LastName = user.LastName;
            userRoles.Phone = user.PhoneNumber;
            userRoles.UserName = user.UserName;
            userRoles.Roles = justAddedRoles;

            return new Response<UserRolesViewModel>(userRoles, message: $"User {user.Email} was successfully added to specified roles.");

        }

       

        public async Task<Response<UserRolesViewModel>> GetUserWithRoles(string userId)
        {
            UserRolesViewModel StaffUserRoles = new UserRolesViewModel();
            var staffUser = await _userManager.FindByIdAsync(userId);

            if (staffUser == null)
            {
                return new Response<UserRolesViewModel>(StaffUserRoles, message: $"User does not exist", isSuccessful: false);
            }
            var staffUserRoles = await GetUserRoles(staffUser);

            StaffUserRoles.FirstName = staffUser.FirstName;
            StaffUserRoles.Email = staffUser.Email;
            StaffUserRoles.LastName = staffUser.LastName;
            StaffUserRoles.Phone = staffUser.PhoneNumber;
            StaffUserRoles.UserName = staffUser.UserName;
            StaffUserRoles.Roles = staffUserRoles;

            return new Response<UserRolesViewModel>(StaffUserRoles, message: $"An error occurred while attempting to remove existing roles.", isSuccessful: false);

        }

        

  

        //for in service use
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.Find(id);
        }
    }

}
