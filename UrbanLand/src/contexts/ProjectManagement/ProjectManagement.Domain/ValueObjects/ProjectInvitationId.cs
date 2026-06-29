using Core.Abstractions;

namespace ProjectManagement.Domain.ValueObjects;

public record ProjectInvitationId(Guid Value) : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static ProjectInvitationId New() => new(Guid.NewGuid());
    public static ProjectInvitationId From(Guid value) => new(value);
}