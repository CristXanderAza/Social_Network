using Microsoft.AspNetCore.Http;
using Social_Network.Core.Application.ViewModels.Posts;
using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Home
{
    public class HomeVM
    {
        public string? UserId { get; set; }
        [Required(ErrorMessage = "This fields is required")]
        public string Content { get; set; }
        public IFormFile? Image { get; set; }

        public string? VideoUrl { get; set; }

        public IEnumerable<Ident<int>>? Types { get; set; }

        [Required(ErrorMessage = "This fields is required")]
        public int PostTypeId { get; set; } = 0;

        public IEnumerable<PostReadVM>? ReadVMs { get; set; }
    }
}
