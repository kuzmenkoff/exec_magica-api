namespace ExecMagica.Domain.Entities;

/// <summary>A card entry within a deck together with its quantity (join entity).</summary>
public class DeckCard
{
    /// <summary>Owning deck id.</summary>
    public int DeckId { get; set; }

    /// <summary>Referenced card id.</summary>
    public int CardId { get; set; }

    /// <summary>How many copies of the card are in the deck.</summary>
    public int Quantity { get; set; } = 1;

    /// <summary>Navigation to the referenced card.</summary>
    public Card? Card { get; set; }
}
