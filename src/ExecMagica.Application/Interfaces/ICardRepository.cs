using ExecMagica.Domain.Entities;

namespace ExecMagica.Application.Interfaces;

/// <summary>
/// Data-access abstraction for cards. Declared in the Application layer and
/// implemented by the Infrastructure layer (dependency inversion).
/// </summary>
public interface ICardRepository
{
    /// <summary>Returns all cards ordered by id.</summary>
    Task<IReadOnlyList<Card>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns a single card by id, or <c>null</c> if it does not exist.</summary>
    Task<Card?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Returns a tracked card (with effects) for editing, or <c>null</c>.</summary>
    Task<Card?> GetForUpdateAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Adds a new card and saves.</summary>
    Task AddAsync(Card card, CancellationToken cancellationToken = default);

    /// <summary>Persists changes to an already-tracked card.</summary>
    Task UpdateAsync(Card card, CancellationToken cancellationToken = default);

    /// <summary>Removes a card and saves.</summary>
    Task DeleteAsync(Card card, CancellationToken cancellationToken = default);
}
