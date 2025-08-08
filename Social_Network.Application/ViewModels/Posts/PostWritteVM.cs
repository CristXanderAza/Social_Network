using Microsoft.AspNetCore.Http;
using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Posts
{
    public class PostWritteVM
    {
        public  string UsertId { get; set; }
        [Required(ErrorMessage = "This fields is required")]
        public  string Content { get; set; }
        public IFormFile? Image { get; set; }

        public string? VideoUrl { get; set; }

        public IEnumerable<Ident<int>> Types { get; set; }

        [Required(ErrorMessage = "This fields is required")]
        public int PostTypeId { get; set; }
    }
}
