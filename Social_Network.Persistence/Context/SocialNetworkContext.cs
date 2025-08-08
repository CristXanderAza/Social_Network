using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using Social_Network.Core.Domain.Entities.Battleship.ShipPositions;
using Social_Network.Core.Domain.Entities.Battleship.Ships;
using Social_Network.Core.Domain.Entities.Comments;
using Social_Network.Core.Domain.Entities.FriendshipRequests;
using Social_Network.Core.Domain.Entities.Friendships;
using Social_Network.Core.Domain.Entities.Posts;
using Social_Network.Core.Domain.Entities.Reactions;
using System.Reflection;

namespace Social_Network.Infraestructure.Persistence.Context
{
    public class SocialNetworkContext : DbContext
    {
        //public DbSet<DomainUser> Users { get; set; }
        public DbSet<Post> Posts {  get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<BattleShipGame> BattleShipGames { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<ShipPosition> ShipPositions { get; set; }
        public DbSet<Attack> Attacks { get; set; }
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
