namespace Social_Network.Core.Application.DTOs.Posts
{
    public class ReactToPostDTO
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }  
        public int ReactionId { get; set; }
    }
}
