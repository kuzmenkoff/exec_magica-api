using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Domain.Entities;

namespace ExecMagica.Application.Services;

/// <summary>
/// Default <see cref="ICardService"/> implementation: reads domain cards via the
/// repository and maps them to <see cref="CardDto"/>.
/// </summary>
public class CardService : ICardService
{
    private readonly ICardRepository cards;

    /// <summary>Initializes the service with a card repository.</summary>
    public CardService(ICardRepository cards)
    {
        this.cards = cards;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CardDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await this.cards.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<CardDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await this.cards.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    /// <summary>Maps a domain <see cref="Card"/> to its <see cref="CardDto"/>.</summary>
    private static CardDto MapToDto(Card card)
    {
        return new CardDto
        {
            Id = card.Id,
            Title = card.Title,
            Description = card.Description,
            ImagePath = card.ImagePath,
            Class = card.Class.ToString(),
            Attack = card.Attack,
            HP = card.HP,
            MaxHP = card.MaxHP,
            ManaCost = card.ManaCost,
            IsCollectible = card.IsCollectible,
            Keywords = card.Keywords.Select(k => k.ToString()).ToList(),
            Effects = card.Effects.Select(MapEffect).ToList(),
        };
    }

    /// <summary>Maps a domain <see cref="CardEffect"/> to its <see cref="CardEffectDto"/>.</summary>
    private static CardEffectDto MapEffect(CardEffect effect)
    {
        return new CardEffectDto
        {
            Trigger = effect.Trigger.ToString(),
            Type = effect.Type.ToString(),
            Target = effect.Target.ToString(),
            Value = effect.Value,
            AttackValue = effect.AttackValue,
            HealthValue = effect.HealthValue,
            SummonCardId = effect.SummonCardId,
            Keyword = effect.Keyword.ToString(),
            Condition = effect.Condition,
        };
    }
}
