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

    /// <inheritdoc />
    public async Task<CardDto> CreateAsync(CardWriteRequest request, CancellationToken cancellationToken = default)
    {
        var card = MapToEntity(request);
        await this.cards.AddAsync(card, cancellationToken);
        return MapToDto(card);
    }

    /// <inheritdoc />
    public async Task<CardDto?> UpdateAsync(int id, CardWriteRequest request, CancellationToken cancellationToken = default)
    {
        var card = await this.cards.GetForUpdateAsync(id, cancellationToken);
        if (card is null)
        {
            return null;
        }

        card.Title = request.Title;
        card.Description = request.Description;
        card.ImagePath = request.ImagePath;
        card.Class = request.Class;
        card.Attack = request.Attack;
        card.HP = request.HP;
        card.MaxHP = request.MaxHP;
        card.ManaCost = request.ManaCost;
        card.IsCollectible = request.IsCollectible;
        card.Keywords = request.Keywords.ToList();

        // Replace the whole effect set (orphans are deleted via cascade).
        card.Effects.Clear();
        foreach (var effect in request.Effects)
        {
            card.Effects.Add(MapToEffect(effect));
        }

        await this.cards.UpdateAsync(card, cancellationToken);
        return MapToDto(card);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var card = await this.cards.GetForUpdateAsync(id, cancellationToken);
        if (card is null)
        {
            return false;
        }

        await this.cards.DeleteAsync(card, cancellationToken);
        return true;
    }

    /// <summary>Maps a write request to a new domain <see cref="Card"/>.</summary>
    private static Card MapToEntity(CardWriteRequest request)
    {
        return new Card
        {
            Title = request.Title,
            Description = request.Description,
            ImagePath = request.ImagePath,
            Class = request.Class,
            Attack = request.Attack,
            HP = request.HP,
            MaxHP = request.MaxHP,
            ManaCost = request.ManaCost,
            IsCollectible = request.IsCollectible,
            Keywords = request.Keywords.ToList(),
            Effects = request.Effects.Select(MapToEffect).ToList(),
        };
    }

    /// <summary>Maps an effect request to a domain <see cref="CardEffect"/>.</summary>
    private static CardEffect MapToEffect(CardEffectRequest request)
    {
        return new CardEffect
        {
            Trigger = request.Trigger,
            Type = request.Type,
            Target = request.Target,
            Value = request.Value,
            AttackValue = request.AttackValue,
            HealthValue = request.HealthValue,
            SummonCardId = request.SummonCardId,
            Keyword = request.Keyword,
            Condition = request.Condition,
        };
    }
}
