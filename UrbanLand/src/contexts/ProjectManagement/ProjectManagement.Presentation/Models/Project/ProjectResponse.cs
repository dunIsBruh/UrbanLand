using ProjectManagement.Presentation.Models.Invitation;
using ProjectManagement.Presentation.Models.ProjectMember;
using ProjectManagement.Presentation.Models.ProjectSettings;

namespace ProjectManagement.Presentation.Models.Project;

/// <summary>
/// Detailed project information including settings, members, and invitations.
/// </summary>
public sealed record ProjectResponse
{
    /// <summary>
    /// Unique identifier of the project.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Name of the project.
    /// </summary>
    /// <example>Central Park</example>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Optional description of the project.
    /// </summary>
    /// <example>A modern urban park with recreational zones</example>
    public string? Description { get; init; }

    /// <summary>
    /// Type of the project.
    /// </summary>
    /// <example>Park</example>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Current status of the project (Active, Archived).
    /// </summary>
    /// <example>Active</example>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Identifier of the project owner.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Display name of the project owner.
    /// </summary>
    /// <example>John Doe</example>
    public string OwnerName { get; init; } = string.Empty;

    /// <summary>
    /// Date and time when the project was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Total number of members in the project.
    /// </summary>
    public int MemberCount { get; init; }

    /// <summary>
    /// Role of the currently authenticated user within the project.
    /// </summary>
    /// <example>Manager</example>
    public string CurrentUserRole { get; init; } = string.Empty;

    /// <summary>
    /// Current project settings.
    /// </summary>
    public ProjectSettingsResponse Settings { get; init; } = null!;

    /// <summary>
    /// List of project members.
    /// </summary>
    public List<MemberResponse> Members { get; init; } = new();

    /// <summary>
    /// List of active invitations for the project.
    /// </summary>
    public List<InvitationResponse> ActiveInvitations { get; init; } = new();
}
