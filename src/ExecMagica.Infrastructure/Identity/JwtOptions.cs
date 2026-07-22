namespace ExecMagica.Infrastructure.Identity;

/// <summary>Strongly-typed JWT settings bound from the "Jwt" configuration section.</summary>
public class JwtOptions
{
    /// <summary>Token issuer.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Intended token audience.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Secret signing key (kept in user-secrets / environment).</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Token lifetime in minutes.</summary>
    public int ExpiryMinutes { get; set; } = 60;
}
