namespace Social_Network.Core.Application.DTOs.Comments
{
    public class CommentInsertDTO
    {
        public required string UserId { get; set; }
        public required Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public required string Content { get; set; }
    }
}
