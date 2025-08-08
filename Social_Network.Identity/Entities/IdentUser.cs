using Microsoft.AspNetCore.Identity;

namespace Social_Network.Infraestructure.Identity.Entities
{
    public class IdentUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string PhotoPath { get; set; }
       
    }
}
