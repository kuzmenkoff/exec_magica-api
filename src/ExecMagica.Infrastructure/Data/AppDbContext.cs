using ExecMagica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the application.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>Initializes the context with the configured options.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>The cards table.</summary>
    public DbSet<Card> Cards => this.Set<Card>();

    /// <summary>Configures entity mappings and constraints.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>(card =>
        {
            card.Property(c => c.Title).HasMaxLength(100).IsRequired();
            card.Property(c => c.Description).HasMaxLength(500);
            card.Property(c => c.ImagePath).HasMaxLength(200);
        });
    }
}