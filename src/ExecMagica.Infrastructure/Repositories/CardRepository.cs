using ExecMagica.Application.Interfaces;
using ExecMagica.Domain.Entities;
using ExecMagica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ICardRepository"/> backed by <see cref="AppDbContext"/>.
/// </summary>
public class CardRepository : ICardRepository
{
    private readonly AppDbContext context;

    /// <summary>Initializes the repository with the database context.</summary>
    public CardRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Card>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.context.Cards
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Card?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Cards
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
