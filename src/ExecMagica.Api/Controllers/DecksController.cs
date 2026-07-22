using System.Security.Claims;
using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExecMagica.Api.Controllers;

/// <summary>Endpoints for managing the current user's own decks.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DecksController : ControllerBase
{
    private readonly IDeckService decks;

    /// <summary>Initializes the controller with the deck service.</summary>
    public DecksController(IDeckService decks)
    {
        this.decks = decks;
    }

    /// <summary>Current authenticated user's id (from the JWT).</summary>
    private string UserId => this.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>Lists the current user's decks.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeckDto>>> GetMine(CancellationToken cancellationToken)
    {
        return this.Ok(await this.decks.GetMyDecksAsync(this.UserId, cancellationToken));
    }

    /// <summary>Gets one of the current user's decks by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeckDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var deck = await this.decks.GetMyDeckAsync(this.UserId, id, cancellationToken);
        return deck is null ? this.NotFound() : this.Ok(deck);
    }

    /// <summary>Creates a new deck for the current user.</summary>
    [HttpPost]
    public async Task<ActionResult<DeckDto>> Create(DeckWriteRequest request, CancellationToken cancellationToken)
    {
        var created = await this.decks.CreateAsync(this.UserId, request, cancellationToken);
        return this.CreatedAtAction(nameof(this.GetById), new { id = created.Id }, created);
    }

    /// <summary>Renames one of the current user's decks.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<DeckDto>> Rename(int id, DeckWriteRequest request, CancellationToken cancellationToken)
    {
        var updated = await this.decks.RenameAsync(this.UserId, id, request, cancellationToken);
        return updated is null ? this.NotFound() : this.Ok(updated);
    }

    /// <summary>Deletes one of the current user's decks.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await this.decks.DeleteAsync(this.UserId, id, cancellationToken);
        return deleted ? this.NoContent() : this.NotFound();
    }
}
