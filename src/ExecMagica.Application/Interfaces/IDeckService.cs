using ExecMagica.Application.Dtos;

namespace ExecMagica.Application.Interfaces;

/// <summary>Operations over the current user's decks (ownership enforced).</summary>
public interface IDeckService
{
    /// <summary>Returns all decks of the given user.</summary>
    Task<IReadOnlyList<DeckDto>> GetMyDecksAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>Returns a deck if it exists and belongs to the user; otherwise <c>null</c>.</summary>
    Task<DeckDto?> GetMyDeckAsync(string userId, int deckId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new deck owned by the user.</summary>
    Task<DeckDto> CreateAsync(string userId, DeckWriteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Renames the user's deck; returns <c>null</c> if not found/owned.</summary>
    Task<DeckDto?> RenameAsync(string userId, int deckId, DeckWriteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes the user's deck; returns <c>false</c> if not found/owned.</summary>
    Task<bool> DeleteAsync(string userId, int deckId, CancellationToken cancellationToken = default);

    /// <summary>Adds copies of a card to the user's deck.</summary>
    Task<DeckOperationResult> AddCardAsync(string userId, int deckId, AddCardToDeckRequest request, CancellationToken cancellationToken = default);

    /// <summary>Removes a card entirely from the user's deck.</summary>
    Task<DeckOperationResult> RemoveCardAsync(string userId, int deckId, int cardId, CancellationToken cancellationToken = default);
}
