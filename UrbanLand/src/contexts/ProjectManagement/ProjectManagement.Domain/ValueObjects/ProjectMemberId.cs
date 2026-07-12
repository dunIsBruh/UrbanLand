using Core.Abstractions;

namespace ProjectManagement.Domain.ValueObjects;

public record ProjectMemberId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static ProjectMemberId New() => new(Guid.NewGuid());
    public static ProjectMemberId From(Guid value) => new(value);
}