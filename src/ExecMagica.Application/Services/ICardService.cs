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

    /// <summary>Creates a new card and returns it.</summary>
    Task<CardDto> CreateAsync(CardWriteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing card; returns the updated card or <c>null</c> if not found.</summary>
    Task<CardDto?> UpdateAsync(int id, CardWriteRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes a card; returns <c>false</c> if it does not exist.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

