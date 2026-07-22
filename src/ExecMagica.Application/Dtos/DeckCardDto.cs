namespace ExecMagica.Application.Dtos;

/// <summary>A card entry inside a deck (card id, title and quantity).</summary>
public class DeckCardDto
{
    /// <summary>Referenced card id.</summary>
    public int CardId { get; set; }

    /// <summary>Referenced card title (for display).</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Number of copies in the deck.</summary>
    public int Quantity { get; set; }
}
