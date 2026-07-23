using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Application.Services;
using ExecMagica.Domain.Entities;
using Moq;
using Xunit;

namespace ExecMagica.Tests;

/// <summary>Unit tests for deck-building rules in <see cref="DeckService"/>.</summary>
public class DeckServiceTests
{
    private const string Owner = "user-1";

    private readonly Mock<IDeckRepository> decks = new();
    private readonly Mock<ICardRepository> cards = new();
    private readonly DeckService service;

    public DeckServiceTests()
    {
        this.service = new DeckService(this.decks.Object, this.cards.Object);
    }

    private Deck DeckWith(params DeckCard[] entries)
    {
        var deck = new Deck { Id = 1, Name = "Test", OwnerUserId = Owner };
        deck.Cards.AddRange(entries);
        this.decks.Setup(r => r.GetByIdAsync(deck.Id, It.IsAny<CancellationToken>())).ReturnsAsync(deck);
        return deck;
    }

    private void SetupCard(Card card) =>
        this.cards.Setup(r => r.GetByIdAsync(card.Id, It.IsAny<CancellationToken>())).ReturnsAsync(card);

    private static Card Collectible(int id) => new() { Id = id, Title = $"Card{id}", IsCollectible = true };

    private static AddCardToDeckRequest Add(int cardId, int qty) => new() { CardId = cardId, Quantity = qty };

    [Fact]
    public async Task AddCard_NotOwner_ReturnsDeckNotFound()
    {
        var deck = new Deck { Id = 1, OwnerUserId = "someone-else" };
        this.decks.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(deck);

        var result = await this.service.AddCardAsync(Owner, 1, Add(1, 1));

        Assert.Equal(DeckOperationStatus.DeckNotFound, result.Status);
    }

    [Fact]
    public async Task AddCard_CardMissing_ReturnsCardNotFound()
    {
        this.DeckWith();
        this.cards.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Card?)null);

        var result = await this.service.AddCardAsync(Owner, 1, Add(99, 1));

        Assert.Equal(DeckOperationStatus.CardNotFound, result.Status);
    }

    [Fact]
    public async Task AddCard_NonCollectible_ReturnsCardNotCollectible()
    {
        this.DeckWith();
        this.SetupCard(new Card { Id = 5, IsCollectible = false });

        var result = await this.service.AddCardAsync(Owner, 1, Add(5, 1));

        Assert.Equal(DeckOperationStatus.CardNotCollectible, result.Status);
    }

    [Fact]
    public async Task AddCard_ThirdCopy_ReturnsCopyLimitExceeded()
    {
        this.DeckWith(new DeckCard { DeckId = 1, CardId = 5, Quantity = 2 });
        this.SetupCard(Collectible(5));

        var result = await this.service.AddCardAsync(Owner, 1, Add(5, 1));

        Assert.Equal(DeckOperationStatus.CopyLimitExceeded, result.Status);
    }

    [Fact]
    public async Task AddCard_ExceedsMaxSize_ReturnsDeckSizeLimitExceeded()
    {
        // 15 distinct cards x2 = 30 (the max).
        var full = Enumerable.Range(1, 15)
            .Select(i => new DeckCard { DeckId = 1, CardId = i, Quantity = 2 })
            .ToArray();
        this.DeckWith(full);
        this.SetupCard(Collectible(99));

        var result = await this.service.AddCardAsync(Owner, 1, Add(99, 1));

        Assert.Equal(DeckOperationStatus.DeckSizeLimitExceeded, result.Status);
    }

    [Fact]
    public async Task AddCard_Valid_AddsCardAndSucceeds()
    {
        var deck = this.DeckWith();
        this.SetupCard(Collectible(5));

        var result = await this.service.AddCardAsync(Owner, 1, Add(5, 2));

        Assert.Equal(DeckOperationStatus.Success, result.Status);
        Assert.Contains(deck.Cards, dc => dc.CardId == 5 && dc.Quantity == 2);
        this.decks.Verify(r => r.UpdateAsync(deck, It.IsAny<CancellationToken>()), Times.Once);
    }
}
