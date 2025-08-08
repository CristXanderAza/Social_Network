using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Infraestructure.Identity.Context;
using Social_Network.Infraestructure.Identity.Entities;
using Social_Network.Infraestructure.Identity.Seeds;
using Social_Network.Infraestructure.Identity.Service;

namespace Social_Network.Infraestructure.Identity
{
    public static class ServiceExtensionIOC
    {
        public static IServiceCollection AddIdentityServiceForWebApp(this IServiceCollection services, IConfiguration configuration)
        {
            GeneralConfig(services, configuration);

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;    

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentityCore<IdentUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(3);
            });
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;

            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(4);
                opt.SlidingExpiration = true;
                opt.LoginPath = "/User";
                opt.AccessDeniedPath = "/User";
            });
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        private static void GeneralConfig(IServiceCollection services, IConfiguration configuration)
        {
            //if(configuration.GetValue<bool>(U))
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IdentityContext>(opt =>
                {
                    opt.EnableSensitiveDataLogging();
                    opt.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));

                }, contextLifetime:ServiceLifetime.Scoped,
                   optionsLifetime: ServiceLifetime.Scoped
            );
        }

        public static async Task RunIdentitySeedAsyn(this IServiceProvider serviceProvider)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var userManager = provider.GetRequiredService<UserManager<IdentUser>>();
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                await DefaultRoles.SeedAsync(roleManager);
                await DefaultIdentUser.SeedAsync(userManager);
                
            }
        }
    }
}
