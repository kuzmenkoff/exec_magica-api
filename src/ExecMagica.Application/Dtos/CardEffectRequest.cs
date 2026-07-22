using ExecMagica.Domain.Enums;

namespace ExecMagica.Application.Dtos;

/// <summary>Input payload describing one effect to attach to a card.</summary>
public class CardEffectRequest
{
    /// <summary>When the effect triggers.</summary>
    public EffectTrigger Trigger { get; set; }

    /// <summary>What the effect does.</summary>
    public EffectType Type { get; set; }

    /// <summary>What the effect targets.</summary>
    public EffectTarget Target { get; set; }

    /// <summary>Main numeric value.</summary>
    public int Value { get; set; }

    /// <summary>Optional attack value (BuffStats).</summary>
    public int AttackValue { get; set; }

    /// <summary>Optional health value (BuffStats).</summary>
    public int HealthValue { get; set; }

    /// <summary>Optional card id to summon.</summary>
    public int? SummonCardId { get; set; }

    /// <summary>Optional keyword (AddKeyword).</summary>
    public KeywordType Keyword { get; set; }

    /// <summary>Optional condition text/key.</summary>
    public string? Condition { get; set; }
}
