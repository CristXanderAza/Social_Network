using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social_Network.Core.Application.Interfaces.Services.Email;
using Social_Network.Core.Domain.Settings;
using Social_Network.Infraestructure.Shared.Email;

namespace Social_Network.Infraestructure.Shared
{
    public static class ServiceExtensionIOC
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
