using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Base
{
    public class Ident<Tid>
    {
        public Tid Id { get; set; }
        public string Name { get; set; }
    }
}
