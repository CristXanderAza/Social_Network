namespace Social_Network.Core.Application.DTOs.Friendships
{
    public class FriendDTO
    {
        public required Guid FriendshipId { get; set; }
        public required string FriendID { get; set; }
        public required string FriendName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ProfileImgURL { get; set; }

    }
}
