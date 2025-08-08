namespace Social_Network.Core.Application.DTOs.Posts
{
    public class PostInsertDTO
    {
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int PostTypeId { get; set; }

    }
}
