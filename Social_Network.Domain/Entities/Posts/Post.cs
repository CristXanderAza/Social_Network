using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Comments;
using Social_Network.Core.Domain.Entities.Reactions;

namespace Social_Network.Core.Domain.Entities.Posts
{
    public class Post : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; } 
        public string? VideoUrl { get; set; }
        public PostType Type { get; set; } 
        public string UserId { get; set; } 
        //public DomainUser User { get; set; } 
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
