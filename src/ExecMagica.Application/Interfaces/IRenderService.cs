using ExecMagica.Application.Dtos;

namespace ExecMagica.Application.Interfaces;

/// <summary>Renders a card into an image representation.</summary>
public interface IRenderService
{
    /// <summary>Builds a standalone SVG document for the given card.</summary>
    string RenderCardSvg(CardDto card);
}
