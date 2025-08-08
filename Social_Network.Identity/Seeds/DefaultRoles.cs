using Microsoft.AspNetCore.Identity;

namespace Social_Network.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("BasicUser"));
        } 
    }
}
