using ExecMagica.Application.Dtos;

namespace ExecMagica.Application.Interfaces;

/// <summary>User registration and login operations.</summary>
public interface IAuthService
{
    /// <summary>Registers a new user in the default role and returns a token.</summary>
    Task<AuthResult> RegisterAsync(RegisterRequest request);

    /// <summary>Validates credentials and returns a token on success.</summary>
    Task<AuthResult> LoginAsync(LoginRequest request);
}
