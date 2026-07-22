using ExecMagica.Domain.Enums;

namespace ExecMagica.Domain.Entities;

/// <summary>
/// A single effect attached to a card. Pure descriptive data — this API stores and
/// serves effects but does not execute them (execution lives in the game's logic layer).
/// </summary>
public class CardEffect
{
    /// <summary>Unique identifier (database primary key).</summary>
    public int Id { get; set; }

    /// <summary>Foreign key to the owning <see cref="Card"/>.</summary>
    public int CardId { get; set; }

    /// <summary>When the effect is triggered.</summary>
    public EffectTrigger Trigger { get; set; }

    /// <summary>What the effect does.</summary>
    public EffectType Type { get; set; }

    /// <summary>What the effect targets.</summary>
    public EffectTarget Target { get; set; }

    /// <summary>Main numeric value (e.g. damage, healing, cards to draw).</summary>
    public int Value { get; set; }

    /// <summary>Optional attack value used by BuffStats effects.</summary>
    public int AttackValue { get; set; }

    /// <summary>Optional health value used by BuffStats effects.</summary>
    public int HealthValue { get; set; }

    /// <summary>Optional card id to summon (used by Summon effects).</summary>
    public int? SummonCardId { get; set; }

    /// <summary>Optional keyword used by AddKeyword effects.</summary>
    public KeywordType Keyword { get; set; }

    /// <summary>Optional condition text/key, reserved for future conditional effects.</summary>
    public string? Condition { get; set; }
}
