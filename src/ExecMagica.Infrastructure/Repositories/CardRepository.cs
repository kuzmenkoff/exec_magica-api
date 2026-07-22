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
            .Include(c => c.Effects)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Card?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Cards
            .AsNoTracking()
            .Include(c => c.Effects)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Card?> GetForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        // Tracked (no AsNoTracking) so EF can persist edits and effect changes.
        return await this.context.Cards
            .Include(c => c.Effects)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Card card, CancellationToken cancellationToken = default)
    {
        await this.context.Cards.AddAsync(card, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Card card, CancellationToken cancellationToken = default)
    {
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Card card, CancellationToken cancellationToken = default)
    {
        this.context.Cards.Remove(card);
        await this.context.SaveChangesAsync(cancellationToken);
    }
}
