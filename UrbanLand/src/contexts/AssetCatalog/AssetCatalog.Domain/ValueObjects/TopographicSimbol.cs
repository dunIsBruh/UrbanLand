using Core.Abstractions;

namespace AssetCatalog.Domain.ValueObjects;

public record TopographicSymbol(string SymbolType, string Color, double Size) : ValueObject
{
    public static TopographicSymbol Default => new("circle", "#000000", 10);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SymbolType;
        yield return Color;
        yield return Size;
    }
}