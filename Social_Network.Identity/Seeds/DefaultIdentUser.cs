using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social_Network.Infraestructure.Identity.Entities;

namespace Social_Network.Infraestructure.Identity.Seeds
{
    public static class DefaultIdentUser
    {
        public static async Task SeedAsync(UserManager<IdentUser> userManager)
        {
            IdentUser user = new()
            {
                Email = "cristopherxanderazadiaz19111@gmail.com",
                UserName = "Placeholder",
                FirstName = "Placeholder",
                LastName = "Placeholder",
                PhoneNumberConfirmed = true,
                PhotoPath = "dada",
                EmailConfirmed = true,
            };

            if ( await userManager.Users.AllAsync(u => u.Id != user.Id)) 
            {
                var userWithEmail = await userManager.FindByEmailAsync(user.Email);
                if (userWithEmail == null)
                {
                    var res = await userManager.CreateAsync(user, "P@ssw0rd123!");
                    if(res.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "BasicUser");
                    }
                    else
                    {
                        var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                        throw new Exception($"User creation failed: {errors}");
                    }
                    
                }


            }
        }
    }
}
