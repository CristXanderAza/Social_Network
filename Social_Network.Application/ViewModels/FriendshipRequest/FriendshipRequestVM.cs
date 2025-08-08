using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.FriendshipRequest
{
    public class FriendshipRequestVM
    {
        public Guid FrienshipRequestId { get; set; }
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string Status { get; set; }
        public int FriendsInCommon { get; set; }
        public bool IsFromUser { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
