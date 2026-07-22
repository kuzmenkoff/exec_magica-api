using System.ComponentModel.DataAnnotations;

namespace ExecMagica.Application.Dtos;

/// <summary>Payload to log in with email and password.</summary>
public class LoginRequest
{
    /// <summary>Account email.</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Account password.</summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
