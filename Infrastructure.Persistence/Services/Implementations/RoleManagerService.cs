using Application.DTOs.Account;
using Application.DTOs.Account.RoleManagement;
using Application.Enums;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly SilverwarePOSEmailSenders _emailSenderAddresses;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRandomNumberGeneratorInterface _randomNumberGenerator;

        public RoleManagerService(UserManager<ApplicationUser> userManager,
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

        public async Task<Response<string>> CreateRole(CreateRoleRequest roleRequest)
        {
            if (roleRequest.role != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleRequest.role.Trim()));
            }

            return new Response<string>($"{roleRequest.role} role was created successfully");
        }

        public async Task<Response<string>> EditRole(string roleId, string name)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return new Response<string>($"Role {role.Name} was edited successfully");
            }
            return null;
        }


        public async Task<Response<List<IdentityRole>>> GetAllRoles()
        {

            var roles = await _roleManager.Roles.ToListAsync();
            return new Response<List<IdentityRole>>(roles, message: $"{ roles.Count().ToString() } roles retrieved.");
        }

        public async Task<Response<IdentityRole>> GetRoleById(string roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);

            return new Response<IdentityRole>(role, message: $"Role found.");
        }



        public async Task<string> AddUserRoleAsync(AddToRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return $"No Accounts Registered with {model.Email}.";
            }

            var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == model.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";

        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.Find(id);
        }
    }

}
