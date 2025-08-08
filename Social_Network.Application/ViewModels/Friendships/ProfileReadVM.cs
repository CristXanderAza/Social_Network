using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Friendships
{
    public class ProfileReadVM
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string ProfileImgURL { get; set; }
        public IEnumerable<PostReadVM>? PostDTOs { get; set; }
    }
}
