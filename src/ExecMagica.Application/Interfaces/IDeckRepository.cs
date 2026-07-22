using ExecMagica.Domain.Entities;

namespace ExecMagica.Application.Interfaces;

/// <summary>Data access for decks.</summary>
public interface IDeckRepository
{
    /// <summary>Returns all decks owned by the given user.</summary>
    Task<IReadOnlyList<Deck>> GetByOwnerAsync(string ownerUserId, CancellationToken cancellationToken = default);

    /// <summary>Returns a tracked deck (with its cards) by id, or <c>null</c>.</summary>
    Task<Deck?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Adds a new deck and saves.</summary>
    Task AddAsync(Deck deck, CancellationToken cancellationToken = default);

    /// <summary>Persists changes to a tracked deck.</summary>
    Task UpdateAsync(Deck deck, CancellationToken cancellationToken = default);

    /// <summary>Removes a deck and saves.</summary>
    Task DeleteAsync(Deck deck, CancellationToken cancellationToken = default);
}
