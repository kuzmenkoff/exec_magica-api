namespace ExecMagica.Domain;

/// <summary>Deck-building rules shared across the system.</summary>
public static class DeckRules
{
    /// <summary>Maximum copies of a single card allowed in a deck.</summary>
    public const int MaxCopiesPerCard = 2;

    /// <summary>Maximum total number of cards allowed in a deck.</summary>
    public const int MaxDeckSize = 30;
}
