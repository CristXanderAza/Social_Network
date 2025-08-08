using Social_Network.Core.Application.DTOs.Comments;

namespace Social_Network.Core.Application.DTOs.Posts
{
    public class PostDTO : BaseResourceDTO<Guid>
    {
        public string AuthorUserName { get; set; }
        public string UserId { get; set; }
        public string UserProfilePhotoPath { get; set; }
        public string Content { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }

        public int Likes { get; set; }
        public bool IsLiked { get; set; } = false;
        public int Dislikes { get; set; }
        public bool IsDisliked { get; set; } = false;
        public DateTime? LastUpdated { get; set; }

        public IEnumerable<CommentDTO>? Comments { get; set; }



    }
}
