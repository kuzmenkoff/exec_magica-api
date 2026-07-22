using ExecMagica.Domain.Enums;

namespace ExecMagica.Domain.Entities;

/// <summary>
/// A collectible card definition: its title, stats, cost, class and keywords.
/// Mirrors the game's card definition and holds no per-match runtime state.
/// </summary>
public class Card
{
    /// <summary>Unique identifier (database primary key).</summary>
    public int Id { get; set; }

    /// <summary>Display name of the card.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Rules text / flavor shown on the card.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Card artwork path/name (same idea as LogoPath in the game).</summary>
    public string? ImagePath { get; set; }

    /// <summary>Whether the card is an entity (creature) or a spell.</summary>
    public CardClass Class { get; set; }

    /// <summary>Attack value.</summary>
    public int Attack { get; set; }

    /// <summary>Current health points.</summary>
    public int HP { get; set; }

    /// <summary>Maximum health points.</summary>
    public int MaxHP { get; set; }

    /// <summary>Mana required to play the card.</summary>
    public int ManaCost { get; set; }

    /// <summary>Whether the card may be added to a deck (tokens/summons are false).</summary>
    public bool IsCollectible { get; set; } = true;

    /// <summary>Passive keywords of the card.</summary>
    public List<KeywordType> Keywords { get; set; } = new();
}
