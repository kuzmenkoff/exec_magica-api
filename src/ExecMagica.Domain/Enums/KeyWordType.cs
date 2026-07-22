namespace ExecMagica.Domain.Enums;

/// <summary>
/// Persistent passive keywords a card can carry. Unlike triggered effects,
/// keywords are always-on properties of the card.
/// </summary>
public enum KeywordType
{
    /// <summary>No keyword.</summary>
    None,

    /// <summary>Enemy cards and hero cannot be attacked while this card is on the battlefield.</summary>
    Provocation,

    /// <summary>The next damage taken by this card is prevented.</summary>
    Shield,

    /// <summary>This card can attack immediately after being played.</summary>
    Charge,

    /// <summary>This card can attack enemy cards immediately, but not the enemy hero on the same turn.</summary>
    Rush,

    /// <summary>This card can attack twice during one turn.</summary>
    DoubleAttack,

    /// <summary>When this card deals damage, its owner's hero is healed.</summary>
    Lifesteal,
}
