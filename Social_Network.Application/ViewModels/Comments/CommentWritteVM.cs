using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Comments
{
    public class CommentWritteVM
    {
        public Guid PostId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; } = null;
        public Guid? CommentId { get; set; }
        public bool IsCreating { get; set; } = true;
    }
}
