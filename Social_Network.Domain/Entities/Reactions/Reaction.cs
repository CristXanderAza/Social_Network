using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Posts;

namespace Social_Network.Core.Domain.Entities.Reactions
{
    public class Reaction : BaseEntity<Guid>
    {
        public ReactionType Type { get; set; }
        public string UserId { get; set; }
       // public DomainUser User { get; set; }
        public Guid PostId { get; set; } 
        public Post Post { get; set; }
    }
}
