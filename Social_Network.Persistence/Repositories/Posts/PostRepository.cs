using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.Posts;
using Social_Network.Core.Domain.Entities.Posts;
using Social_Network.Infraestructure.Persistence.Context;

namespace Social_Network.Infraestructure.Persistence.Repositories.Posts
{
    public class PostRepository : BaseRepository<Post, Guid>, IPostRepository
    {
        public PostRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsOfFriendsOfUser(string userId)
        {
            var friendIds = _context.Friendships
                            .Where(f => f.FromUserId == userId || f.ToUserId == userId)
                            .Select(f => f.FromUserId == userId ? f.ToUserId : f.FromUserId);
            return await _context.Posts
                .Where(P => friendIds.Contains(P.UserId))
                .Include(p => p.Reactions)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsOfUser(string userId)
            => await _context.Posts.Where(p => p.UserId == userId)
                    .Include(p => p.Reactions)
                    .ToListAsync();    

        public async Task<Post> GetPostWithCommentsAndReactions(Guid postId)
            => await _context.Posts.Where(p => p.Id == postId)
                            .Include(p => p.Reactions)
                            .Include(p => p.Comments)
                            .FirstOrDefaultAsync();
    }
}
