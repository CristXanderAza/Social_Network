namespace Social_Network.Core.Application.DTOs.FriendshipRequest
{
    public class FriendshipRequestDTO
    {
        public Guid FrienshipRequestId { get; set; }
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }   
        public string Status { get; set; }
        public int FriendsInCommon { get; set; }
        public bool IsFromUser { get; set; }
        public DateTime RequestDate { get; set; }

    }
}
