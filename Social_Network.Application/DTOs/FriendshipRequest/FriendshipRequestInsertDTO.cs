using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.FriendshipRequest
{
    public class FriendshipRequestInsertDTO
    {
        public required string FromUserId { get; set; }
        public required string ToUserId { get; set; }
    }
}
