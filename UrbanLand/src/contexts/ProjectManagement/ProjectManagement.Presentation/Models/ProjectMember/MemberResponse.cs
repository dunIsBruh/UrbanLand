namespace ProjectManagement.Presentation.Models.ProjectMember;

public sealed record MemberResponse
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    // public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public DateTime JoinedAt { get; init; }
    public Guid? InvitedBy { get; init; }
}
