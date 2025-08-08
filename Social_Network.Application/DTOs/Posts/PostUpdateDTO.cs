namespace Social_Network.Core.Application.DTOs.Posts
{
    public class PostUpdateDTO : BaseResourceDTO<Guid>
    {
        public required string Content { get; set; }
    }
}
