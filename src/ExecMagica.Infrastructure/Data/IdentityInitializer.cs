using ExecMagica.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExecMagica.Infrastructure.Data;

/// <summary>Seeds the Admin/User roles and a default admin account at startup.</summary>
public static class IdentityInitializer
{
    private static readonly string[] RoleNames = { "Admin", "User" };

    /// <summary>Ensures roles exist and creates the admin account if it is missing.</summary>
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        string? adminEmail,
        string? adminPassword)
    {
        foreach (var role in RoleNames)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            Email = adminEmail,
            UserName = adminEmail,
            EmailConfirmed = true,
        };

        var created = await userManager.CreateAsync(admin, adminPassword);
        if (created.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
