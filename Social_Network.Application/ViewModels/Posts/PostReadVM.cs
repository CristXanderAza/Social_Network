using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Application.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Posts
{
    public class PostReadVM
    {
        public string AuthorUserName { get; set; }
        public Guid Id {  get; set; }
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

        public IEnumerable<CommentReadVM>? Comments { get; set; }
    }
}
