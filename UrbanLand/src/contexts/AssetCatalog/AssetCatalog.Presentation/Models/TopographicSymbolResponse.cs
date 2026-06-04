namespace AssetCatalog.Presentation.Models;

public sealed record TopographicSymbolResponse
{
    public string SymbolType { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public double Size { get; init; }
}
