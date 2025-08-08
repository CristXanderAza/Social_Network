namespace Social_Network.Core.Application.ViewModels.Comments
{
    public class CommentListVM
    {
        public Guid PostId { get; set; }
        public string ActualUserId { get; set; }
        public string PostText { get; set; }
        public string UserName { get; set; }
        public string UserProfilePhotoUrl { get; set; }
        public string? VideoUrl { get; set; } 
        public string? ImgUrl { get; set; }
        public IEnumerable<CommentReadVM> ReadVMs { get; set; } = Enumerable.Empty<CommentReadVM>();
    }
}
