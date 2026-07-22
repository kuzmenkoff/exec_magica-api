namespace ExecMagica.Application.Dtos;

/// <summary>
/// API-facing representation of a card. Enums are exposed as strings so the
/// contract stays readable and decoupled from internal storage.
/// </summary>
public class CardDto
{
    /// <summary>Unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Display name of the card.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Rules text / flavor.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Card artwork path/name.</summary>
    public string? ImagePath { get; set; }

    /// <summary>Card class as a string ("Entity" / "Spell").</summary>
    public string Class { get; set; } = string.Empty;

    /// <summary>Attack value.</summary>
    public int Attack { get; set; }

    /// <summary>Current health points.</summary>
    public int HP { get; set; }

    /// <summary>Maximum health points.</summary>
    public int MaxHP { get; set; }

    /// <summary>Mana cost.</summary>
    public int ManaCost { get; set; }

    /// <summary>Whether the card may be added to a deck.</summary>
    public bool IsCollectible { get; set; }

    /// <summary>Passive keywords as strings.</summary>
    public IReadOnlyList<string> Keywords { get; set; } = Array.Empty<string>();
}
