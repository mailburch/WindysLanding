using Microsoft.AspNetCore.Identity;
using WindysLanding.Models;

namespace WindysLanding.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Emails to be assigned to Admin Credentials (replace with actual emails of team members)
            string[] adminEmails = new[]
            {
                "crazeebra@gmail.com",        // ← Liam
                "Nguyen49@mail.nmc.edu",          // ← Rico
                "vittorh@mail.nmc.edu",          // ← Hope
                "cadealpersbusiness@gmail.com"           // ← Cade
            };

            // Make each email an admin
            foreach (var email in adminEmails)
            {
                var adminUser = await userManager.FindByEmailAsync(email);

                // If user exists and is not already admin, make them admin
                if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}