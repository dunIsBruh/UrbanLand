using Core.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record SceneId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static SceneId New() => new(Guid.NewGuid());
    public static SceneId From(Guid value) => new(value);
}