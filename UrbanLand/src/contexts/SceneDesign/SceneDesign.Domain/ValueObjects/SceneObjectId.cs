using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record SceneObjectId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static SceneObjectId New() => new(Guid.NewGuid());
    public static SceneObjectId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}