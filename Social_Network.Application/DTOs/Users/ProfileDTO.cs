using Social_Network.Core.Application.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Users
{
    public class ProfileDTO
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string ProfileImgURL { get; set; }
        //public required int CommonFriendship { get; set; }
        public IEnumerable<PostDTO>? PostDTOs { get; set; }
    }
}
