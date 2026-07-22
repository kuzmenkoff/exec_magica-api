using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExecMagica.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ExecMagica.Infrastructure.Identity;

/// <summary>Default <see cref="IJwtTokenGenerator"/> using HMAC-SHA256 signing.</summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions options;

    /// <summary>Initializes the generator with JWT settings.</summary>
    public JwtTokenGenerator(JwtOptions options)
    {
        this.options = options;
    }

    /// <inheritdoc />
    public string GenerateToken(string userId, string email, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: this.options.Issuer,
            audience: this.options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(this.options.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
