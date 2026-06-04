using SharedKernel.Abstractions;

namespace SharedKernel.Contracts;

public record ProjectId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static ProjectId New() => new(Guid.NewGuid());
    public static ProjectId From(Guid value) => new(value);
}