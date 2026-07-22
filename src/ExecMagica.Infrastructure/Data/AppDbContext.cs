using ExecMagica.Domain.Entities;
using ExecMagica.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context, including ASP.NET Core Identity tables.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>Initializes the context with the configured options.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>The cards table.</summary>
    public DbSet<Card> Cards => this.Set<Card>();

    /// <summary>The card effects table.</summary>
    public DbSet<CardEffect> CardEffects => this.Set<CardEffect>();

    /// <summary>The decks table.</summary>
    public DbSet<Deck> Decks => this.Set<Deck>();

    /// <summary>Configures entity mappings and constraints.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>(card =>
        {
            card.Property(c => c.Title).HasMaxLength(100).IsRequired();
            card.Property(c => c.Description).HasMaxLength(500);
            card.Property(c => c.ImagePath).HasMaxLength(200);

            // One card has many effects; deleting a card removes its effects.
            card.HasMany(c => c.Effects)
                .WithOne()
                .HasForeignKey(e => e.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CardEffect>(effect =>
        {
            effect.Property(e => e.Condition).HasMaxLength(200);
        });

        modelBuilder.Entity<Deck>(deck =>
        {
            deck.Property(d => d.Name).HasMaxLength(100).IsRequired();
            deck.Property(d => d.OwnerUserId).IsRequired();

            // A deck has many card entries; deleting the deck removes them.
            deck.HasMany(d => d.Cards)
                .WithOne()
                .HasForeignKey(dc => dc.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            // A deck belongs to one Identity user; deleting the user removes their decks.
            deck.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeckCard>(deckCard =>
        {
            deckCard.HasKey(dc => new { dc.DeckId, dc.CardId });

            deckCard.HasOne(dc => dc.Card)
                .WithMany()
                .HasForeignKey(dc => dc.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}