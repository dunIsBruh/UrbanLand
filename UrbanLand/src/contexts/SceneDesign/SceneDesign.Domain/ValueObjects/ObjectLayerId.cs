using SharedKernel.Abstractions;

namespace SceneDesign.Domain.ValueObjects;

public record ObjectLayerId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static ObjectLayerId New() => new(Guid.NewGuid());
    public static ObjectLayerId From(Guid value) => new(value);
}
