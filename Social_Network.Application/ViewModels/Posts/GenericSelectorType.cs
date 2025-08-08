using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Posts
{
    public class GenericSelectorType<TId>
    {
        public IEnumerable<Ident<TId>> Options { get; set; }

        [Required(ErrorMessage = "You must select an option")]       
        public TId SelectedOption { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }  
        public string PageName { get; set; }
        public string PageTitle { get; set; }
    }
}
