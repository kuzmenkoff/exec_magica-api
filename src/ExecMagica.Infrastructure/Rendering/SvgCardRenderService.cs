using System.Globalization;
using System.Net;
using System.Text;
using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;

namespace ExecMagica.Infrastructure.Rendering;

/// <summary>
/// Renders a card by sandwiching the art between two Unity-rendered templates
/// (back = frame/body, front = banners/orbs/emblem) and drawing the text on top,
/// using the game's Peaberry font and the exact prefab text positions.
/// </summary>
public class SvgCardRenderService : IRenderService
{
    private const int W = 800;
    private const int H = 1118;

    // Crop to the templates' content (union alpha bbox) — removes transparent margin.
    private const int CropX = 183, CropY = 181, CropW = 507, CropH = 697;

    // Art window (normalized, from the prefab's Logo rect).
    private const double ArtX = 0.336, ArtY = 0.220, ArtW = 0.460, ArtH = 0.329;

    /// <inheritdoc />
    public string RenderCardSvg(CardDto card)
    {
        var isSpell = string.Equals(card.Class, "Spell", StringComparison.OrdinalIgnoreCase);
        var front = isSpell ? "spell_front.png" : "entity_front.png";

        var sb = new StringBuilder();
        sb.Append(FormattableString.Invariant(
            $"<svg xmlns='http://www.w3.org/2000/svg' viewBox='{CropX} {CropY} {CropW} {CropH}' width='{CropW}' height='{CropH}'>"));

        // Embed the game font.
        sb.Append("<style>@font-face{font-family:'Peaberry';src:url('/fonts/Peaberry.ttf');}text{font-family:'Peaberry',monospace;}</style>");

        // 1) Back template.
        FullImage(sb, "/cards/card_back.png");

        // 2) Card art, clipped to the art window (optional).
        if (!string.IsNullOrWhiteSpace(card.ImagePath))
        {
            double ax = ArtX * W, ay = ArtY * H, aw = ArtW * W, ah = ArtH * H;
            sb.Append(FormattableString.Invariant(
                $"<clipPath id='art'><rect x='{ax:F1}' y='{ay:F1}' width='{aw:F1}' height='{ah:F1}'/></clipPath>"));
            sb.Append(FormattableString.Invariant(
                $"<image href='{Escape(card.ImagePath)}' x='{ax:F1}' y='{ay:F1}' width='{aw:F1}' height='{ah:F1}' preserveAspectRatio='xMidYMid slice' clip-path='url(#art)'/>"));
        }

        // 3) Front template.
        FullImage(sb, $"/cards/{front}");

        // 4) Text (positions/colors from the prefab).
        // Name — black, on the title banner.
        Text(sb, Escape(card.Title), 0.5635, 0.510, 36, "#000000", stroke: 3);

        // Description — black, wrapped, vertically centered on its point.
        double dcx = 0.5655 * W, dcy = 0.644 * H, dw = 380, dh = 240;
        sb.Append(FormattableString.Invariant(
            $"<foreignObject x='{dcx - dw / 2:F1}' y='{dcy - dh / 2:F1}' width='{dw:F1}' height='{dh:F1}'>"));
        sb.Append("<div xmlns='http://www.w3.org/1999/xhtml' style=\"height:100%;display:flex;align-items:center;justify-content:center;\">");
        sb.Append("<div style=\"font-family:'Peaberry',monospace;font-size:30px;line-height:1.2;color:#000;text-align:center;\">");
        sb.Append(Escape(card.Description));
        sb.Append("</div></div></foreignObject>");

        // Stat numbers — white, on the gems.
        Text(sb, card.ManaCost.ToString(CultureInfo.InvariantCulture), 0.3040, 0.221, 80, "#ffffff", stroke: 7);
        if (!isSpell)
        {
            Text(sb, card.Attack.ToString(CultureInfo.InvariantCulture), 0.3375, 0.730, 80, "#ffffff", stroke: 7);
            Text(sb, card.HP.ToString(CultureInfo.InvariantCulture), 0.8005, 0.730, 80, "#ffffff", stroke: 7);
        }

        sb.Append("</svg>");
        return sb.ToString();
    }

    private static void FullImage(StringBuilder sb, string href)
    {
        sb.Append(FormattableString.Invariant(
            $"<image href='{href}' x='0' y='0' width='{W}' height='{H}' preserveAspectRatio='none'/>"));
    }

    private static void Text(StringBuilder sb, string value, double ncx, double ncy, int size, string color, int stroke = 0)
    {
        var strokeAttr = stroke > 0
            ? $" paint-order='stroke' stroke='#000000' stroke-width='{stroke}' stroke-linejoin='round'"
            : string.Empty;
        sb.Append(FormattableString.Invariant(
            $"<text x='{ncx * W:F1}' y='{ncy * H:F1}' text-anchor='middle' dominant-baseline='central' font-size='{size}' fill='{color}'{strokeAttr}>{value}</text>"));
    }

    private static string Escape(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);
}