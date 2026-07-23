using System.Text.Json;
using ExecMagica.Domain.Entities;
using ExecMagica.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExecMagica.Infrastructure.Data;

/// <summary>Applies migrations and seeds the card catalog from the embedded game JSON.</summary>
public static class DbInitializer
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>Migrates the database and imports all cards if the table is empty.</summary>
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Cards.AnyAsync())
        {
            return;
        }

        context.Cards.AddRange(LoadCards());
        await context.SaveChangesAsync();
    }

    /// <summary>Reads and maps every card from the embedded JSON resources.</summary>
    private static List<Card> LoadCards()
    {
        var assembly = typeof(DbInitializer).Assembly;
        var cards = new List<Card>();

        foreach (var name in assembly.GetManifestResourceNames()
                     .Where(n => n.Contains("CardData") && n.EndsWith(".json", StringComparison.OrdinalIgnoreCase)))
        {
            using var stream = assembly.GetManifestResourceStream(name)!;
            var set = JsonSerializer.Deserialize<CardSetJson>(stream, JsonOptions);
            if (set is null)
            {
                continue;
            }

            foreach (var card in set.Entities.Concat(set.Spells).Concat(set.SummonEntities))
            {
                cards.Add(MapCard(card));
            }
        }

        return cards;
    }

    /// <summary>Maps a JSON card to a domain <see cref="Card"/>.</summary>
    private static Card MapCard(CardJson c)
    {
        return new Card
        {
            Title = c.Title,
            Description = c.Description,
            ImagePath = ToImagePath(c.LogoPath),
            Class = ParseEnum(c.Class, CardClass.Entity),
            Attack = c.Attack,
            HP = c.HP,
            MaxHP = c.MaxHP,
            ManaCost = c.ManaCost,
            IsCollectible = c.IsCollectible,
            Keywords = c.Keywords.Select(k => ParseEnum(k, KeywordType.None)).ToList(),
            Effects = c.Effects.Select(MapEffect).ToList(),
        };
    }

    /// <summary>Maps a JSON effect to a domain <see cref="CardEffect"/>.</summary>
    private static CardEffect MapEffect(EffectJson e)
    {
        return new CardEffect
        {
            Trigger = ParseEnum(e.Trigger, EffectTrigger.OnPlay),
            Type = ParseEnum(e.Type, EffectType.None),
            Target = ParseEnum(e.Target, EffectTarget.None),
            Value = e.Value,
            AttackValue = e.AttackValue,
            HealthValue = e.HealthValue,
            SummonCardId = e.CardId,
            Keyword = ParseEnum(e.Keyword, KeywordType.None),
            Condition = e.Condition,
        };
    }

    /// <summary>Case-insensitive enum parse with a fallback for unknown values.</summary>
    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback)
        where TEnum : struct =>
        Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed) ? parsed : fallback;

    /// <summary>Converts a game LogoPath to an art URL, e.g. "/art/Name.png".</summary>
    private static string? ToImagePath(string? logoPath)
    {
        if (string.IsNullOrWhiteSpace(logoPath))
        {
            return null;
        }

        var fileName = logoPath.Split('/')[^1];
        return "/art/" + Uri.EscapeDataString(fileName) + ".jpg";
    }
}
