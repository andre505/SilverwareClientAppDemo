using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Shared.LoggerClass;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            //services.AddSingleton<IMandrillEmailService, MailgunEmailService>();
            services.AddTransient<IRandomNumberGeneratorInterface, RandomNumberGeneratorService>();
            services.AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<ApplicationLogs>>());

        }
    }
}
