using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.FriendshipRequest
{
    public class FriendOptionVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public int FriendsInCommon { get; set; }
    }
}
