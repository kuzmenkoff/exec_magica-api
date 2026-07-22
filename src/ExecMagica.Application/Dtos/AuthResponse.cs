namespace ExecMagica.Application.Dtos;

/// <summary>Successful authentication result returned to the client.</summary>
public class AuthResponse
{
    /// <summary>Signed JWT access token.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Authenticated user's email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Roles granted to the user.</summary>
    public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
}
