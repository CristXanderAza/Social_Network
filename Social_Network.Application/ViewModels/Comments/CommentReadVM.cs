using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Comments
{
    public class CommentReadVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserPhotoPath { get; set; }
        public required string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
