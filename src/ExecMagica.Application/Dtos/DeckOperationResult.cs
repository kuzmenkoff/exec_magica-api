namespace ExecMagica.Application.Dtos;

/// <summary>Possible outcomes of a deck-card operation.</summary>
public enum DeckOperationStatus
{
    /// <summary>The operation succeeded.</summary>
    Success,

    /// <summary>The deck does not exist or is not owned by the user.</summary>
    DeckNotFound,

    /// <summary>The card does not exist or is not in the deck.</summary>
    CardNotFound,

    /// <summary>The card exists but cannot be placed in a deck.</summary>
    CardNotCollectible,

    /// <summary>Adding would exceed the per-card copy limit.</summary>
    CopyLimitExceeded,

    /// <summary>Adding would exceed the maximum deck size.</summary>
    DeckSizeLimitExceeded,
}

/// <summary>Outcome of a deck-card operation, with the resulting deck on success.</summary>
public class DeckOperationResult
{
    /// <summary>The outcome status.</summary>
    public DeckOperationStatus Status { get; private set; }

    /// <summary>The updated deck when the operation succeeded.</summary>
    public DeckDto? Deck { get; private set; }

    /// <summary>Creates a successful result carrying the updated deck.</summary>
    public static DeckOperationResult Success(DeckDto deck) =>
        new() { Status = DeckOperationStatus.Success, Deck = deck };

    /// <summary>Creates a result for the given failure status.</summary>
    public static DeckOperationResult Fail(DeckOperationStatus status) =>
        new() { Status = status };
}
