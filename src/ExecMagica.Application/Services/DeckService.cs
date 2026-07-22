using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Domain.Entities;

namespace ExecMagica.Application.Services;

/// <summary>Default <see cref="IDeckService"/>; enforces that users touch only their own decks.</summary>
public class DeckService : IDeckService
{
    private readonly IDeckRepository decks;

    /// <summary>Initializes the service with the deck repository.</summary>
    public DeckService(IDeckRepository decks)
    {
        this.decks = decks;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DeckDto>> GetMyDecksAsync(string userId, CancellationToken cancellationToken = default)
    {
        var owned = await this.decks.GetByOwnerAsync(userId, cancellationToken);
        return owned.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<DeckDto?> GetMyDeckAsync(string userId, int deckId, CancellationToken cancellationToken = default)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        return IsOwned(deck, userId) ? MapToDto(deck!) : null;
    }

    /// <inheritdoc />
    public async Task<DeckDto> CreateAsync(string userId, DeckWriteRequest request, CancellationToken cancellationToken = default)
    {
        var deck = new Deck
        {
            Name = request.Name,
            OwnerUserId = userId,
        };

        await this.decks.AddAsync(deck, cancellationToken);
        return MapToDto(deck);
    }

    /// <inheritdoc />
    public async Task<DeckDto?> RenameAsync(string userId, int deckId, DeckWriteRequest request, CancellationToken cancellationToken = default)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        if (!IsOwned(deck, userId))
        {
            return null;
        }

        deck!.Name = request.Name;
        await this.decks.UpdateAsync(deck, cancellationToken);
        return MapToDto(deck);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string userId, int deckId, CancellationToken cancellationToken = default)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        if (!IsOwned(deck, userId))
        {
            return false;
        }

        await this.decks.DeleteAsync(deck!, cancellationToken);
        return true;
    }

    /// <summary>Returns whether the deck exists and is owned by the given user.</summary>
    private static bool IsOwned(Deck? deck, string userId) =>
        deck is not null && deck.OwnerUserId == userId;

    /// <summary>Maps a domain <see cref="Deck"/> to its <see cref="DeckDto"/>.</summary>
    private static DeckDto MapToDto(Deck deck)
    {
        return new DeckDto
        {
            Id = deck.Id,
            Name = deck.Name,
            Cards = deck.Cards
                .Select(dc => new DeckCardDto
                {
                    CardId = dc.CardId,
                    Title = dc.Card?.Title ?? string.Empty,
                    Quantity = dc.Quantity,
                })
                .ToList(),
        };
    }
}
