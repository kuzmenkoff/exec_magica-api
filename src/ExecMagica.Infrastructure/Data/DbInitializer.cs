using ExecMagica.Domain.Entities;
using ExecMagica.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Data;

/// <summary>
/// Applies pending migrations and seeds initial data at application startup.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Migrates the database and inserts a starter set of cards if the table is empty.
    /// </summary>
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Cards.AnyAsync())
        {
            return;
        }

        context.Cards.AddRange(
            new Card
            {
                Title = "Footman",
                Description = "A sturdy frontline soldier.",
                Class = CardClass.Entity,
                Attack = 2,
                HP = 3,
                MaxHP = 3,
                ManaCost = 2,
                Keywords = new List<KeywordType> { KeywordType.Provocation },
            },
            new Card
            {
                Title = "Fireball",
                Description = "Deal 3 damage to a target.",
                Class = CardClass.Spell,
                Attack = 0,
                HP = 0,
                MaxHP = 0,
                ManaCost = 4,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Trigger = EffectTrigger.OnPlay,
                        Type = EffectType.DealDamage,
                        Target = EffectTarget.SelectedEnemyCharacter,
                        Value = 3,
                    },
                },
            });

        await context.SaveChangesAsync();
    }
}
