using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Entities;

public class ProjectMember : Entity<ProjectMemberId>
{
    private ProjectMember() { }

    private ProjectMember(
        ProjectMemberId id,
        UserId memberId, 
        ProjectId projectId,
        ProjectRole role,
        UserId? invitedBy
        ) : base(id)
    {
        MemberId = memberId;
        ProjectId = projectId;
        Role = role;
        InvitedBy = invitedBy;
        JoinedAt = DateTime.UtcNow;
    }

    public UserId MemberId { get; private set; } = null!;
    public ProjectId ProjectId { get; private set; } = null!;
    public ProjectRole Role { get; private set; } = null!;
    public UserId? InvitedBy { get; private set; }
    public DateTime JoinedAt { get; private set; }
    
    public static ProjectMember Create(
        UserId memberId,
        ProjectId projectId, 
        ProjectRole role, 
        UserId? invitedBy
        )
        => new(ProjectMemberId.New(), memberId, projectId, role, invitedBy);
        
    public static ProjectMember CreateOwner(UserId memberId, ProjectId projectId)
        => new(ProjectMemberId.New(), memberId, projectId, ProjectRole.RoleManager, null);
        
    public void ChangeRole(ProjectRole role) => Role = role;
}