using Microsoft.AspNetCore.Identity;

namespace ExecMagica.Infrastructure.Identity;

/// <summary>
/// Application user, extending the ASP.NET Core Identity user.
/// Custom profile fields (and deck ownership) can be added here later.
/// </summary>
public class ApplicationUser : IdentityUser
{
}
