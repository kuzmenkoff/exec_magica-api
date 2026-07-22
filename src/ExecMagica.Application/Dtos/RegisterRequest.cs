using System.ComponentModel.DataAnnotations;

namespace ExecMagica.Application.Dtos;

/// <summary>Payload to register a new account.</summary>
public class RegisterRequest
{
    /// <summary>Account email (also used as the username).</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Account password.</summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
