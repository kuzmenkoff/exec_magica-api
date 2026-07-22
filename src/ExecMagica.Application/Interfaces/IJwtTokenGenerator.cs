namespace ExecMagica.Application.Interfaces;

/// <summary>Creates signed JWT access tokens for authenticated users.</summary>
public interface IJwtTokenGenerator
{
    /// <summary>Generates a signed JWT carrying the user's id, email and roles.</summary>
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
}
