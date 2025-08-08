using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Common
{
    public class GenericAlertVM
    {
        public Guid ResourceId { get; set; }
        public Guid? AnotherResoruceId { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string ActionSource { get; set; } = "Index";
        public string ActionDestination { get; set; }
        public string alertType { get; set; }  = "danger";
    }
}
