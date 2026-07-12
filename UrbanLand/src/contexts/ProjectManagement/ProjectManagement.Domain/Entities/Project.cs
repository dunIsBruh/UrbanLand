using Core.Abstractions;
using Core.Identity;
using Core.Primitives;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Domain.Events;
using ProjectManagement.Domain.ValueObjects;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Entities;

public class Project : AggregateRoot<ProjectId>
{
    private readonly List<ProjectMember> _members = [];
    private readonly List<ProjectInvitation> _invitations = [];
    
    public string Name { get; private set; }
    public ProjectSettings Settings { get; private set; }
    public UserId OwnerId { get; }
    public string? Description { get; private set; }
    public ProjectStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public IReadOnlyList<ProjectMember> Members => _members.AsReadOnly();
    public IReadOnlyList<ProjectInvitation> Invitations => _invitations.AsReadOnly();
    
    private Project(ProjectId id, string name, UserId ownerId)
        : base(id)
    {
        Name = name;
        OwnerId = ownerId;
        Settings = ProjectSettings.CreateDefault();
        Status = ProjectStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        
        _members.Add(ProjectMember.CreateOwner(ownerId, Id));
    }
    
    public static Result<Project> Create(string name, UserId ownerId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Project>.Failure(Error.Validation("Project name is required"));
        }
        
        if (name.Length > 200)
        {
            return Result<Project>.Failure(Error.Validation("Project name must be less than 200 characters"));
        }

        var project = new Project(ProjectId.New(), name, ownerId)
        {
            Description = description
        };
        
        project.AddDomainEvent(new ProjectCreatedDomainEvent(project.Id, project.Name, project.OwnerId));
        
        return Result<Project>.Success(project);
    }
    
    public Result AddMember(UserId userId, ProjectRole role, UserId invitedBy)
    {
        if (!HasAccess(invitedBy, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only managers can add members"));
        }

        if (_members.Any(m => m.MemberId == userId))
        {
            return Result.Failure(Error.Conflict("User already a member"));
        }

        _members.Add(ProjectMember.Create(userId, Id, role, invitedBy));
        return Result.Success();
    }
    
    public Result ChangeRole(UserId userId, ProjectRole newRole, UserId changedBy)
    {
        if (!HasAccess(changedBy, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only managers can change roles"));
        }
        
        var member = _members.FirstOrDefault(m => m.MemberId == userId);
        if (member == null)
        {
            return Result.Failure(Error.NotFound("Member", userId));
        }

        // Нельзя удалить последнего менеджера
        if (member.Role.CanManageRoles() && !HasAnyManagersBesideCurrent(userId))
        {
            return Result.Failure(Error.Validation("Project must have at least one manager"));
        }

        member.ChangeRole(newRole);
        return Result.Success();
    }
    
    public Result RemoveMember(UserId userId, UserId removedBy)
    {
        if (!HasAccess(removedBy, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only managers can remove members"));
        }

        if (userId == OwnerId)
        {
            return Result.Failure(Error.Forbidden("Cannot remove the project owner"));
        }

        var member = _members.FirstOrDefault(m => m.MemberId == userId);
        if (member == null)
        {
            return Result.Failure(Error.NotFound("Member", userId));
        }

        _members.Remove(member);
        AddDomainEvent(new MemberRemovedDomainEvent(Id, userId));
        return Result.Success();
    }

    public Result Archive(UserId userId)
    {
        if (!HasAccess(userId, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only managers can archive projects"));
        }

        if (Status == ProjectStatus.Archived)
        {
            return Result.Failure(Error.Conflict("Project is already archived"));
        }

        var oldStatus = Status;
        Status = ProjectStatus.Archived;
        AddDomainEvent(new ProjectStatusChangedDomainEvent(Id, oldStatus, Status));
        return Result.Success();
    }

    public Result<ProjectInvitation> CreateInvitation(ProjectRole suggestedRole, UserId createdBy)
    {
        if (!HasAccess(createdBy, p => p.CanManageRoles()))
        {
            return Result<ProjectInvitation>.Failure(Error.Forbidden("Only managers can create invitations"));
        }

        var invitation = ProjectInvitation.Create(Id, suggestedRole, Settings.InvitationValidityPeriod);
        _invitations.Add(invitation);
        return Result<ProjectInvitation>.Success(invitation);
    }
    
    public Result<ProjectRole> AcceptInvitation(string inviteCode, UserId userId)
    {
        var invitation = _invitations.FirstOrDefault(i => i.InviteCode == inviteCode);
        if (invitation == null)
        {
            return Result<ProjectRole>.Failure(Error.NotFound("Invitation", inviteCode));
        }

        if (!invitation.IsValid())
        {
            return Result<ProjectRole>.Failure(Error.Validation("Invitation expired or already used"));
        }

        if (_members.Any(m => m.MemberId == userId))
        {
            return Result<ProjectRole>.Failure(Error.Conflict("User already a member"));
        }

        invitation.MarkAsUsed();
        var role = invitation.SuggestedRole;
        _members.Add(ProjectMember.Create(userId, Id, role, null));
    
        return Result<ProjectRole>.Success(role);
    }
    
    public Result UpdateSettings(ProjectSettings newSettings, UserId changedBy)
    {
        if (!HasAccess(changedBy, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only managers can change project settings"));
        }

        Settings = newSettings;
        return Result.Success();
    }
    
    public bool HasAccess(UserId userId, Func<ProjectRole, bool> permissionCheck)
    {
        if (userId == OwnerId)
        {
            return true;
        }

        var member = _members.FirstOrDefault(m => m.MemberId == userId);
        return member != null && permissionCheck(member.Role);
    }
    
    
    private bool HasAnyManagersBesideCurrent(UserId userId)
    {
        return _members.Any(m => 
            m.MemberId != userId && 
            m.Role.CanManageRoles());
    }
}
