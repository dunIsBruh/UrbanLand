using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Entities;

public class ProjectInvitation : Entity<ProjectInvitationId>
{
    private ProjectInvitation() { }

    private ProjectInvitation(
        ProjectInvitationId id,
        ProjectId projectId,
        ProjectRole suggestedRole,
        TimeSpan validityPeriod)
        : base(id)
    {
        ProjectId = projectId;
        SuggestedRole = suggestedRole;
        ExpiresAt = DateTime.UtcNow.Add(validityPeriod);
    }

    public ProjectId ProjectId { get; private set; } = default!;
    public string InviteCode { get; private set; } = GenerateCode();
    public ProjectRole SuggestedRole { get; private set; } = default!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    public static ProjectInvitation Create(ProjectId projectId, ProjectRole suggestedRole, TimeSpan validityPeriod) 
        => new(ProjectInvitationId.New(), projectId, suggestedRole, validityPeriod);
    
    public bool IsValid() => !IsUsed && DateTime.UtcNow <= ExpiresAt;

    public void MarkAsUsed() => IsUsed = true;
    
    private static string GenerateCode() => Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..8];
}