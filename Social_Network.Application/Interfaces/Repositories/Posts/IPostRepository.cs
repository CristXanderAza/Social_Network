using Social_Network.Core.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Posts
{
    public interface IPostRepository : IRepositoryBase<Post, Guid>
    {
        Task<IEnumerable<Post>> GetPostsOfFriendsOfUser(string userId);
        Task<IEnumerable<Post>> GetPostsOfUser(string userId);
        Task<Post> GetPostWithCommentsAndReactions(Guid postId);
    }
}
