using Core.Abstractions;

namespace AssetCatalog.Domain.ValueObjects;

public record Dimensions : ValueObject
{
    public double Width { get; }
    public double Height { get; }
    public double Depth { get; }

    public Dimensions(double width, double height, double depth)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depth);

        Width = width;
        Height = height;
        Depth = depth;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
        yield return Depth;
    }
}