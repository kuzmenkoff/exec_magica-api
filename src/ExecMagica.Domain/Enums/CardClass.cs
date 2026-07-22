namespace ExecMagica.Domain.Enums;

/// <summary>
/// The card's fundamental type, mirroring the game's card classes.
/// </summary>
public enum CardClass
{
    /// <summary>A creature summoned to the battlefield that can attack.</summary>
    Entity,

    /// <summary>A one-shot card that resolves its effects and never enters the field.</summary>
    Spell,
}
