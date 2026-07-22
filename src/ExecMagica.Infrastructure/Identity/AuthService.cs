using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExecMagica.Infrastructure.Identity;

/// <summary>Handles registration and login using Identity, issuing JWT tokens.</summary>
public class AuthService : IAuthService
{
    private const string DefaultRole = "User";

    private readonly UserManager<ApplicationUser> userManager;
    private readonly IJwtTokenGenerator tokenGenerator;

    /// <summary>Initializes the service with the user manager and token generator.</summary>
    public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenGenerator tokenGenerator)
    {
        this.userManager = userManager;
        this.tokenGenerator = tokenGenerator;
    }

    /// <inheritdoc />
    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (await this.userManager.FindByEmailAsync(request.Email) is not null)
        {
            return AuthResult.Fail("A user with this email already exists.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
        };

        var created = await this.userManager.CreateAsync(user, request.Password);
        if (!created.Succeeded)
        {
            return AuthResult.Fail(created.Errors.Select(e => e.Description).ToArray());
        }

        await this.userManager.AddToRoleAsync(user, DefaultRole);

        return await this.BuildSuccessAsync(user);
    }

    /// <inheritdoc />
    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await this.userManager.FindByEmailAsync(request.Email);
        if (user is null || !await this.userManager.CheckPasswordAsync(user, request.Password))
        {
            return AuthResult.Fail("Invalid email or password.");
        }

        return await this.BuildSuccessAsync(user);
    }

    /// <summary>Builds a successful result with a freshly issued token.</summary>
    private async Task<AuthResult> BuildSuccessAsync(ApplicationUser user)
    {
        var roles = await this.userManager.GetRolesAsync(user);
        var token = this.tokenGenerator.GenerateToken(user.Id, user.Email!, roles);

        return AuthResult.Success(new AuthResponse
        {
            Token = token,
            Email = user.Email!,
            Roles = roles.ToArray(),
        });
    }
}
