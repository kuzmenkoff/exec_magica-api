namespace ExecMagica.Domain.Entities;

/// <summary>A user-owned deck: a named collection of cards.</summary>
public class Deck
{
    /// <summary>Unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Deck display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Identity user id of the owner.</summary>
    public string OwnerUserId { get; set; } = string.Empty;

    /// <summary>Cards contained in the deck, with quantities.</summary>
    public List<DeckCard> Cards { get; set; } = new();
}
