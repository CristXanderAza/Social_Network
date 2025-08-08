using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Posts;

namespace Social_Network.Core.Application.Interfaces.Services.Posts
{
    public interface IPostService : IServiceBase<Post, Guid, PostDTO, PostInsertDTO, PostUpdateDTO>
    {
        Task<Result<Unit>> ReactToPost(ReactToPostDTO reacDTO);
        Task<IEnumerable<PostDTO>> GetFriendsPosts(string userId);
        Task<IEnumerable<PostDTO>> GetPostsOfUser(string userId);
        Task<ProfileDTO> GetProfileOfUser(string userId);
        Task<PostDTO> GetPostWithCommentsAndReactions(Guid postId);
    }
}
