using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities.FriendshipRequests
{
    public class FriendshipRequest : BaseEntity<Guid>
    {
        public string FromUserId { get; set; }
       // public DomainUser FromUser { get; set; } 
        public string ToUserId { get; set; }
        //public DomainUser ToUser { get; set; }
        public FriendshipRequestStatus Status { get; set; } = FriendshipRequestStatus.Pending;
        public DateTime? ResponseDate { get; set; } 
        public int CommonFriendsCount { get; set; } = 0;

    }
}
