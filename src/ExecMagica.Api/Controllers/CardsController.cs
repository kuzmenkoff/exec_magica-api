using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using ExecMagica.Application.Services;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IRenderService renderer;

    /// <summary>Initializes the controller with the card service and renderer.</summary>
    public CardsController(ICardService cards, IRenderService renderer)
    {
        this.cards = cards;
        this.renderer = renderer;
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

    /// <summary>Creates a new card. Admin only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CardDto>> Create(CardWriteRequest request, CancellationToken cancellationToken)
    {
        var created = await this.cards.CreateAsync(request, cancellationToken);
        return this.CreatedAtAction(nameof(this.GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing card. Admin only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CardDto>> Update(int id, CardWriteRequest request, CancellationToken cancellationToken)
    {
        var updated = await this.cards.UpdateAsync(id, request, cancellationToken);
        return updated is null ? this.NotFound() : this.Ok(updated);
    }

    /// <summary>Deletes a card. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await this.cards.DeleteAsync(id, cancellationToken);
        return deleted ? this.NoContent() : this.NotFound();
    }

    /// <summary>Renders the card as an SVG image (public).</summary>
    [HttpGet("{id:int}/render")]
    [Produces("image/svg+xml")]
    public async Task<IActionResult> Render(int id, CancellationToken cancellationToken)
    {
        var card = await this.cards.GetByIdAsync(id, cancellationToken);
        if (card is null)
        {
            return this.NotFound();
        }

        return this.Content(this.renderer.RenderCardSvg(card), "image/svg+xml");
    }
}
