using ExecMagica.Application.Interfaces;
using ExecMagica.Domain.Entities;
using ExecMagica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Repositories;

/// <summary>EF Core implementation of <see cref="IDeckRepository"/>.</summary>
public class DeckRepository : IDeckRepository
{
    private readonly AppDbContext context;

    /// <summary>Initializes the repository with the database context.</summary>
    public DeckRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Deck>> GetByOwnerAsync(string ownerUserId, CancellationToken cancellationToken = default)
    {
        return await this.context.Decks
            .AsNoTracking()
            .Include(d => d.Cards)
                .ThenInclude(dc => dc.Card)
            .Where(d => d.OwnerUserId == ownerUserId)
            .OrderBy(d => d.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Deck?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await this.context.Decks
            .Include(d => d.Cards)
                .ThenInclude(dc => dc.Card)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Deck deck, CancellationToken cancellationToken = default)
    {
        await this.context.Decks.AddAsync(deck, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Deck deck, CancellationToken cancellationToken = default)
    {
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Deck deck, CancellationToken cancellationToken = default)
    {
        this.context.Decks.Remove(deck);
        await this.context.SaveChangesAsync(cancellationToken);
    }
}
