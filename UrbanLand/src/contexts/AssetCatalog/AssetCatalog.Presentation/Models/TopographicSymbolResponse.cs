namespace AssetCatalog.Presentation.Models;

/// <summary>
/// Topographic symbol data returned in list responses.
/// </summary>
public sealed record TopographicSymbolResponse
{
    /// <summary>
    /// Type of the topographic symbol.
    /// </summary>
    /// <example>Tree</example>
    public string SymbolType { get; init; } = string.Empty;

    /// <summary>
    /// Color of the symbol in hex format.
    /// </summary>
    /// <example>#00FF00</example>
    public string Color { get; init; } = string.Empty;

    /// <summary>
    /// Size of the symbol in map units.
    /// </summary>
    /// <example>1.5</example>
    public double Size { get; init; }
}
