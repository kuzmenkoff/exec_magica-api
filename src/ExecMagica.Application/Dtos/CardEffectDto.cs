namespace ExecMagica.Application.Dtos;

/// <summary>
/// API-facing representation of a card effect. Enums exposed as strings.
/// </summary>
public class CardEffectDto
{
    /// <summary>When the effect triggers ("OnPlay", "OnDeath", ...).</summary>
    public string Trigger { get; set; } = string.Empty;

    /// <summary>What the effect does ("DealDamage", "Heal", ...).</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>What the effect targets.</summary>
    public string Target { get; set; } = string.Empty;

    /// <summary>Main numeric value.</summary>
    public int Value { get; set; }

    /// <summary>Optional attack value (BuffStats).</summary>
    public int AttackValue { get; set; }

    /// <summary>Optional health value (BuffStats).</summary>
    public int HealthValue { get; set; }

    /// <summary>Optional card id to summon.</summary>
    public int? SummonCardId { get; set; }

    /// <summary>Optional keyword (AddKeyword).</summary>
    public string Keyword { get; set; } = string.Empty;

    /// <summary>Optional condition text/key.</summary>
    public string? Condition { get; set; }
}
