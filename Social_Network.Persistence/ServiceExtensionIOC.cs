using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Domain.Base;
using Social_Network.Infraestructure.Persistence.Context;
using Social_Network.Infraestructure.Persistence.Repositories.Posts;
using System.Reflection;

namespace Social_Network.Infraestructure.Persistence
{
    public static class ServiceExtensionIOC
    {

        public static IServiceCollection AddStereotype(
            this IServiceCollection services, Type type,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var Implementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                             i.GetGenericTypeDefinition() == type))
                .ToList();

            foreach (var implementation in Implementations)
            {
                var concreteInterface = implementation.GetInterfaces()
                    .FirstOrDefault(i => !i.IsGenericType &&
                                       i.GetInterfaces()
                                        .Any(gi => gi.IsGenericType &&
                                                  gi.GetGenericTypeDefinition() == type));

                if (concreteInterface != null)
                {
                    services.Add(new ServiceDescriptor(
                        concreteInterface,
                        implementation,
                        lifetime));
                }
            }

            return services;
        }

        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SocialNetworkContext>(opt =>
            {
                opt.EnableSensitiveDataLogging();
                opt.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(SocialNetworkContext).Assembly.FullName));

            }, contextLifetime: ServiceLifetime.Scoped,
                   optionsLifetime: ServiceLifetime.Scoped
            );

            /*
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IFriendshipRequestRepository, FriendshipRequestRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IShipRepository, ShipRepository>();
            services.AddScoped<IBattleShipGameRepository, BattleShipGameRepository>();
            services.AddScoped<IAttackRepository, AttackRepository>();
            */
            services.AddStereotype(typeof(IRepositoryBase<,>),Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
