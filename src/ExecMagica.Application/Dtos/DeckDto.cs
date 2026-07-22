namespace ExecMagica.Application.Dtos;

/// <summary>API representation of a deck and its cards.</summary>
public class DeckDto
{
    /// <summary>Unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Deck display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Cards in the deck.</summary>
    public IReadOnlyList<DeckCardDto> Cards { get; set; } = Array.Empty<DeckCardDto>();
}