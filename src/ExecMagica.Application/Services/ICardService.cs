using ExecMagica.Application.Dtos;

namespace ExecMagica.Application.Services;

/// <summary>
/// Application-level operations over cards, returning API DTOs.
/// </summary>
public interface ICardService
{
    /// <summary>Returns all cards as DTOs.</summary>
    Task<IReadOnlyList<CardDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns a single card DTO by id, or <c>null</c> if not found.</summary>
    Task<CardDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
