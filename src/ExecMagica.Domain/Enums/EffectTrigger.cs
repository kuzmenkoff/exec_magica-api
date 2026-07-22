namespace ExecMagica.Domain.Enums;

/// <summary>
/// Defines when a card effect should be triggered.
/// </summary>
public enum EffectTrigger
{
    /// <summary>Triggered when the card is played (default for spells).</summary>
    OnPlay,

    /// <summary>Triggered when the card dies.</summary>
    OnDeath,

    /// <summary>Triggered at the beginning of the owner's turn.</summary>
    OnTurnStart,

    /// <summary>Triggered after this card deals damage.</summary>
    OnDamageDeal,

    /// <summary>Triggered after this card takes damage.</summary>
    OnDamageTake,
}
