using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record AssetId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}