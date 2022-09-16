using Application.Enums;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = "davidsilverware@silverware.com",
                    Email = "davidsilverware@silverware.com",
                    FirstName = "David",
                    LastName = "Silverware",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                if (userManager.Users.All(u => u.Id != defaultUser.Id))
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                        await userManager.AddToRoleAsync(defaultUser, Roles.Employee.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
