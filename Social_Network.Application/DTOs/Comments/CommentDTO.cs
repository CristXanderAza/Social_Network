namespace Social_Network.Core.Application.DTOs.Comments
{
    public class CommentDTO : BaseResourceDTO<Guid>
    {

        public required string UserName { get; set; }
        public required string UserPhotoPath { get; set; }
        public required string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
