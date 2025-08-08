using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Friendships
{
    public class FriendReadVM
    {
        public required Guid FriendshipId { get; set; }
        public required string FriendID { get; set; }
        public required string FriendName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public required string ProfileImgURL { get; set; }
    }
}
