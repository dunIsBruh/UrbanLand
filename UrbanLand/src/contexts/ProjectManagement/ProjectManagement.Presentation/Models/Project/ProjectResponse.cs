using ProjectManagement.Presentation.Models.Invitation;
using ProjectManagement.Presentation.Models.ProjectMember;
using ProjectManagement.Presentation.Models.ProjectSettings;

namespace ProjectManagement.Presentation.Models.Project;

public sealed record ProjectResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public Guid OwnerId { get; init; }
    public string OwnerName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public int MemberCount { get; init; }
    public string CurrentUserRole { get; init; } = string.Empty;
    public ProjectSettingsResponse Settings { get; init; } = null!;
    public List<MemberResponse> Members { get; init; } = new();
    public List<InvitationResponse> ActiveInvitations { get; init; } = new();
}
