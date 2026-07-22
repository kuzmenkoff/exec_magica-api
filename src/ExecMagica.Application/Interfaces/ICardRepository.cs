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
}
