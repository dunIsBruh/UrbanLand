using Core.Abstractions;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Domain.ValueObjects;

public record ProjectSettings : ValueObject
{
    public bool IsPublic { get; }
    public bool AllowPublicComments { get; }
    public AccessLevel DefaultAccessLevel { get; }
    public TimeSpan InvitationValidityPeriod { get; }
    
    private ProjectSettings(
        bool isPublic,
        bool allowPublicComments,
        AccessLevel defaultAccessLevel,
        TimeSpan invitationValidityPeriod)
    {
        IsPublic = isPublic;
        AllowPublicComments = allowPublicComments;
        DefaultAccessLevel = defaultAccessLevel;
        InvitationValidityPeriod = invitationValidityPeriod;
    }
    
    public static ProjectSettings CreateDefault()
    {
        return new ProjectSettings(
            false, 
            true, 
            AccessLevel.Visitor, 
            TimeSpan.FromDays(7)
            );
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IsPublic;
        yield return AllowPublicComments;
        yield return DefaultAccessLevel;
        yield return InvitationValidityPeriod;
    }
}
