using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Domain.Entities.Friendships
{
    public class Friendship : BaseEntity<Guid>
    {
        public string FromUserId { get; set; }
        //public DomainUser FromUser { get; set; }
        public string ToUserId { get; set; }
       // public DomainUser ToUser { get; set; }
    }
}
