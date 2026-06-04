namespace ProjectManagement.Presentation.Models.ProjectMember;

public sealed record UserProjectResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string UserRole { get; init; } = string.Empty;
    public int MemberCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }
}