using System.ComponentModel.DataAnnotations;

namespace ExecMagica.Application.Dtos;

/// <summary>Input payload to create or rename a deck.</summary>
public class DeckWriteRequest
{
    /// <summary>Deck display name.</summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
