using System.ComponentModel.DataAnnotations;

namespace ExecMagica.Application.Dtos;

/// <summary>Input payload to add copies of a card to a deck.</summary>
public class AddCardToDeckRequest
{
    /// <summary>Id of the card to add.</summary>
    [Range(1, int.MaxValue)]
    public int CardId { get; set; }

    /// <summary>How many copies to add.</summary>
    [Range(1, 99)]
    public int Quantity { get; set; } = 1;
}
