using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Application.Services;
using ExecMagica.Domain.Entities;
using ExecMagica.Domain.Enums;
using Moq;
using Xunit;

namespace ExecMagica.Tests;

/// <summary>Unit tests for mapping and lookups in <see cref="CardService"/>.</summary>
public class CardServiceTests
{
    private readonly Mock<ICardRepository> repo = new();
    private readonly CardService service;

    public CardServiceTests()
    {
        this.service = new CardService(this.repo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_MapsEnumsToStrings()
    {
        var card = new Card
        {
            Id = 1,
            Title = "Fireball",
            Class = CardClass.Spell,
            ManaCost = 4,
            Keywords = new List<KeywordType> { KeywordType.Charge },
        };
        this.repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(card);

        var dto = await this.service.GetByIdAsync(1);

        Assert.NotNull(dto);
        Assert.Equal("Spell", dto!.Class);
        Assert.Equal(new[] { "Charge" }, dto.Keywords);
        Assert.Equal(4, dto.ManaCost);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        this.repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Card?)null);

        Assert.Null(await this.service.GetByIdAsync(123));
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        this.repo.Setup(r => r.GetForUpdateAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Card?)null);

        Assert.False(await this.service.DeleteAsync(123));
    }

    [Fact]
    public async Task CreateAsync_AssignsIdAndMaps()
    {
        var request = new CardWriteRequest { Title = "Knight", Class = CardClass.Entity, Attack = 3, HP = 4, ManaCost = 3 };
        this.repo.Setup(r => r.AddAsync(It.IsAny<Card>(), It.IsAny<CancellationToken>()))
            .Callback<Card, CancellationToken>((c, _) => c.Id = 7)
            .Returns(Task.CompletedTask);

        var dto = await this.service.CreateAsync(request);

        Assert.Equal(7, dto.Id);
        Assert.Equal("Knight", dto.Title);
        Assert.Equal("Entity", dto.Class);
    }
}
