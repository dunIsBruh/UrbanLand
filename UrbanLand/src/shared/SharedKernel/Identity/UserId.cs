using SharedKernel.Abstractions;

namespace SharedKernel.Identity;

public record UserId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);
}