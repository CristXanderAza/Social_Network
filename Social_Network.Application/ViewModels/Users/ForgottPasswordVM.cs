using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Users
{
    public class ForgottPasswordVM
    {
        [Required(ErrorMessage ="This field is required")]
        public string Email { get; set; }
    }
}
