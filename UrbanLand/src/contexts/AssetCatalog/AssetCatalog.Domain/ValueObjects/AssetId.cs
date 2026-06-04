using SharedKernel.Abstractions;

namespace AssetCatalog.Domain.ValueObjects;

public record AssetId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static AssetId New() => new(Guid.NewGuid());
    public static AssetId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}