using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social_Network.Core.Application.Interfaces.Services.Battleship;
using Social_Network.Core.Application.Interfaces.Services.Comments;
using Social_Network.Core.Application.Interfaces.Services.Friendships;
using Social_Network.Core.Application.Interfaces.Services.Posts;
using Social_Network.Core.Application.Services.Battleship;
using Social_Network.Core.Application.Services.Comments;
using Social_Network.Core.Application.Services.Friendships;
using Social_Network.Core.Application.Services.Posts;
using System.Reflection;

namespace Social_Network.Core.Application
{
    public static class ServiceExtensionIOC
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IBattleShipService, BattleShipService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
