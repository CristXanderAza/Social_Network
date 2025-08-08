using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Posts;

namespace Social_Network.Core.Domain.Entities.Comments
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public string UserId { get; set; }
      //  public DomainUser User { get; set; }
        public Guid PostId { get; set; } 
        public Post Post { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
