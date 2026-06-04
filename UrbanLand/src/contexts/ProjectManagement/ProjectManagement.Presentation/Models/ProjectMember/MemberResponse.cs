namespace ProjectManagement.Presentation.Models.ProjectMember;

/// <summary>
/// Information about a project member.
/// </summary>
public sealed record MemberResponse
{
    /// <summary>
    /// Identifier of the member user.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Display name of the member.
    /// </summary>
    /// <example>John Doe</example>
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// Role of the member within the project.
    /// </summary>
    /// <example>Manager</example>
    public string Role { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the member joined the project.
    /// </summary>
    public DateTime JoinedAt { get; init; }

    /// <summary>
    /// Identifier of the user who invited this member, if any.
    /// </summary>
    public Guid? InvitedBy { get; init; }
}
