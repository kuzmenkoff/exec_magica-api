using ExecMagica.Application.Dtos;
using ExecMagica.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
}
