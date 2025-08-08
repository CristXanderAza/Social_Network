using Social_Network.Core.Application.ViewModels.Posts;

namespace Social_Network.Core.Application.ViewModels.Friendships
{
    public class FriendshipMainVM
    {
        public IEnumerable<FriendReadVM> Friends { get; set; }
        public IEnumerable<PostReadVM> Posts { get; set; }
    }
}
