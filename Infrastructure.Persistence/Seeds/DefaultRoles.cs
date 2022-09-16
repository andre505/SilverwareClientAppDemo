using Application.Enums;
using Domain.Entities.Identity;
using Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            //Create Super Admin Role
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));


            //Create Admin Role
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));


            //Create Client Role
            await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
            //Add Moderator Claim


            //Create Employee Role
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
            var employeeRole = await roleManager.FindByNameAsync(Roles.Employee.ToString());
        }
    }
}
