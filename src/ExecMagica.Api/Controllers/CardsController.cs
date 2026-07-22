using ExecMagica.Application.Dtos;
using ExecMagica.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExecMagica.Api.Controllers;

/// <summary>
/// Read endpoints for browsing the card catalog.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ICardService cards;

    /// <summary>Initializes the controller with the card service.</summary>
    public CardsController(ICardService cards)
    {
        this.cards = cards;
    }

    /// <summary>Returns the full list of cards.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CardDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await this.cards.GetAllAsync(cancellationToken);
        return this.Ok(result);
    }

    /// <summary>Returns a single card by id, or 404 if it does not exist.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CardDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var card = await this.cards.GetByIdAsync(id, cancellationToken);
        return card is null ? this.NotFound() : this.Ok(card);
    }
}
