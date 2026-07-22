using System.ComponentModel.DataAnnotations;
using ExecMagica.Domain.Enums;

namespace ExecMagica.Application.Dtos;

/// <summary>Input payload to create or update a card (used by admin endpoints).</summary>
public class CardWriteRequest
{
    /// <summary>Display name.</summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Rules text / flavor.</summary>
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Card artwork path/name.</summary>
    [MaxLength(200)]
    public string? ImagePath { get; set; }

    /// <summary>Entity or Spell.</summary>
    public CardClass Class { get; set; }

    /// <summary>Attack value.</summary>
    [Range(0, int.MaxValue)]
    public int Attack { get; set; }

    /// <summary>Current health points.</summary>
    [Range(0, int.MaxValue)]
    public int HP { get; set; }

    /// <summary>Maximum health points.</summary>
    [Range(0, int.MaxValue)]
    public int MaxHP { get; set; }

    /// <summary>Mana cost.</summary>
    [Range(0, int.MaxValue)]
    public int ManaCost { get; set; }

    /// <summary>Whether the card may be added to a deck.</summary>
    public bool IsCollectible { get; set; } = true;

    /// <summary>Passive keywords.</summary>
    public List<KeywordType> Keywords { get; set; } = new();

    /// <summary>Effects attached to the card.</summary>
    public List<CardEffectRequest> Effects { get; set; } = new();
}
