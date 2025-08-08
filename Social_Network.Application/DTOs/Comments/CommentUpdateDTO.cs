namespace Social_Network.Core.Application.DTOs.Comments
{
    public class CommentUpdateDTO : BaseResourceDTO<Guid>
    {
        public required string Content { get; set; }
    }
}
