namespace Social_Network.Core.Application.ViewModels.FriendshipRequest
{
    public class FriendshipRequestMainVM
    {
        public IEnumerable<FriendshipRequestVM> RequestFrom {  get; set; } = new List<FriendshipRequestVM>();
        public IEnumerable<FriendshipRequestVM> RequestTo { get; set; } = new List<FriendshipRequestVM>();
    }
}
