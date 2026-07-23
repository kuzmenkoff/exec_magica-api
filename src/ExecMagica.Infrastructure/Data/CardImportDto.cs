namespace ExecMagica.Infrastructure.Data;

/// <summary>One card set file (entities/spells/summon tokens) as stored in the game JSON.</summary>
internal sealed class CardSetJson
{
    /// <summary>Collectible creature cards.</summary>
    public List<CardJson> Entities { get; set; } = new();

    /// <summary>Spell cards.</summary>
    public List<CardJson> Spells { get; set; } = new();

    /// <summary>Summon-only token cards.</summary>
    public List<CardJson> SummonEntities { get; set; } = new();
}

/// <summary>A single card as stored in the game JSON.</summary>
internal sealed class CardJson
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Class { get; set; } = "ENTITY";
    public string? LogoPath { get; set; }
    public int Attack { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int ManaCost { get; set; }
    public bool IsCollectible { get; set; }
    public List<string> Keywords { get; set; } = new();
    public List<EffectJson> Effects { get; set; } = new();
}

/// <summary>A single card effect as stored in the game JSON.</summary>
internal sealed class EffectJson
{
    public string Trigger { get; set; } = "OnPlay";
    public string Type { get; set; } = "None";
    public string Target { get; set; } = "None";
    public int Value { get; set; }
    public int AttackValue { get; set; }
    public int HealthValue { get; set; }

    /// <summary>Summon target card id in the game (maps to our SummonCardId).</summary>
    public int? CardId { get; set; }

    public string Keyword { get; set; } = "None";
    public string? Condition { get; set; }
}
