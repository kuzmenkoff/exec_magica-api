using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Domain.Entities;
using ExecMagica.Domain;

namespace ExecMagica.Application.Services;

/// <summary>Default <see cref="IDeckService"/>; enforces that users touch only their own decks.</summary>
public class DeckService : IDeckService
{
    private readonly IDeckRepository decks;
    private readonly ICardRepository cards;

    /// <summary>Initializes the service with the deck and card repositories.</summary>
    public DeckService(IDeckRepository decks, ICardRepository cards)
    {
        this.decks = decks;
        this.cards = cards;
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

    /// <inheritdoc />
    public async Task<DeckOperationResult> AddCardAsync(string userId, int deckId, AddCardToDeckRequest request, CancellationToken cancellationToken = default)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        if (!IsOwned(deck, userId))
        {
            return DeckOperationResult.Fail(DeckOperationStatus.DeckNotFound);
        }

        var card = await this.cards.GetByIdAsync(request.CardId, cancellationToken);
        if (card is null)
        {
            return DeckOperationResult.Fail(DeckOperationStatus.CardNotFound);
        }

        if (!card.IsCollectible)
        {
            return DeckOperationResult.Fail(DeckOperationStatus.CardNotCollectible);
        }

        var entry = deck!.Cards.FirstOrDefault(dc => dc.CardId == request.CardId);

        // Rule: at most MaxCopiesPerCard copies of a single card.
        var copiesAfter = (entry?.Quantity ?? 0) + request.Quantity;
        if (copiesAfter > DeckRules.MaxCopiesPerCard)
        {
            return DeckOperationResult.Fail(DeckOperationStatus.CopyLimitExceeded);
        }

        // Rule: at most MaxDeckSize cards total.
        var totalAfter = deck.Cards.Sum(dc => dc.Quantity) + request.Quantity;
        if (totalAfter > DeckRules.MaxDeckSize)
        {
            return DeckOperationResult.Fail(DeckOperationStatus.DeckSizeLimitExceeded);
        }

        if (entry is null)
        {
            deck.Cards.Add(new DeckCard
            {
                DeckId = deck.Id,
                CardId = request.CardId,
                Quantity = request.Quantity,
            });
        }
        else
        {
            entry.Quantity += request.Quantity;
        }

        await this.decks.UpdateAsync(deck, cancellationToken);
        return DeckOperationResult.Success(await this.ReloadDtoAsync(deckId, cancellationToken));
    }

    /// <inheritdoc />
    public async Task<DeckOperationResult> RemoveCardAsync(string userId, int deckId, int cardId, CancellationToken cancellationToken = default)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        if (!IsOwned(deck, userId))
        {
            return DeckOperationResult.Fail(DeckOperationStatus.DeckNotFound);
        }

        var entry = deck!.Cards.FirstOrDefault(dc => dc.CardId == cardId);
        if (entry is null)
        {
            return DeckOperationResult.Fail(DeckOperationStatus.CardNotFound);
        }

        deck.Cards.Remove(entry);
        await this.decks.UpdateAsync(deck, cancellationToken);
        return DeckOperationResult.Success(await this.ReloadDtoAsync(deckId, cancellationToken));
    }

    /// <summary>Reloads the deck (with card details) and maps it to a DTO.</summary>
    private async Task<DeckDto> ReloadDtoAsync(int deckId, CancellationToken cancellationToken)
    {
        var deck = await this.decks.GetByIdAsync(deckId, cancellationToken);
        return MapToDto(deck!);
    }
}
