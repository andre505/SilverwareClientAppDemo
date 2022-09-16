using Application.Enums;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           

            //Seed Admin
            var superAdminUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "anthony.odu@hotmail.com",
                FirstName = "Anthony",
                LastName = "Odu",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Id != superAdminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(superAdminUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superAdminUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(superAdminUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(superAdminUser, Roles.SuperAdmin.ToString());   
                }
            }
        }
    }
}
