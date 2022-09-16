using Domain.Entities.Identity;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Domain.Entities.Identity;
using Infrastructure.Persistence.Helpers;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace SilverwarePOS
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();


            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();


                    //Seed Default Roles
                    if (!roleManager.Roles.Any())
                    {
                        await Infrastructure.Persistence.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                    }

                    
                    Log.Information("Finished Seeding Default Data");

                    //Seed Default Users
                    if (!userManager.Users.Any())
                    {
                        await Infrastructure.Persistence.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
                        await Infrastructure.Persistence.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                    }

                    //Seed Default Entities
                    dbInitializer.Initialize();
                    dbInitializer.SeedDefaultEntities();
                    Log.Information("Application Starting");
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "An error occurred seeding the DB");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            host.Run();
            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              //.ConfigureAppConfiguration((context, config) =>
              //{
              //var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("SilverwarePOSVaultUri"));
              //config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
              //})
              .UseSerilog() 
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacContainerModule());
                });
    }
}
