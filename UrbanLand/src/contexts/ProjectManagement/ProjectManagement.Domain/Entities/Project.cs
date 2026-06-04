using ProjectManagement.Domain.Enums;
using ProjectManagement.Domain.Events;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

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

// public class Project : AggregateRoot<ProjectId>
// {
//     private readonly List<ProjectMember> _members = [];
//     private readonly List<ProjectInvitation> _invitations = [];
//     
//     public string Name { get; private set; }
//     public ProjectSettings Settings { get; private set; }
//     public UserId OwnerId { get; }
//     public string? Description { get; private set; }
//     public ProjectStatus Status { get; private set; }
//     public DateTime CreatedAt { get; private set; }
//     // public SceneId ActiveSceneId { get; private set; }
//     public IReadOnlyList<ProjectMember> Members => _members;
//     public IReadOnlyList<ProjectInvitation> Invitations => _invitations;
//     
//     private Project(ProjectId id, string name, UserId ownerId)
//         : base(id)
//     {
//         Name = name;
//         OwnerId = ownerId;
//         Settings = ProjectSettings.CreateDefault();
//         
//         _members.Add(ProjectMember.CreateOwner(ownerId, Id));
//     }
//     
//     public static Result<Project> Create(string name, UserId ownerId, string? description = null)
//     {
//         if (string.IsNullOrWhiteSpace(name))
//         {
//              throw new DomainException("Project name is required");
//         }
//         
//         if (name.Length > 200)
//         {
//              throw new DomainException("Project name must be less than 200 characters");
//         }
//
//         var project = new Project(ProjectId.New(), name, ownerId)
//         {
//             Description = description
//         };
//         
//         project.AddDomainEvent(new ProjectCreatedDomainEvent(project.Id, project.Name, project.OwnerId));
//         
//         return Result<Project>.Success(project);
//     }
//     
//     public Result AddMember(UserId userId, ProjectRole role, UserId invitedBy)
//     {
//         // Проверка прав: только Manager может добавлять участников
//         if (!HasPermission(invitedBy, p => p.CanManageRoles()))
//         {
//             return Result.Failure(Error.Forbidden("Only managers can add members"));
//         }
//
//         if (_members.Any(m => m.UserId == userId))
//         {
//             return Result.Failure(Error.Conflict("User already a member"));
//         }
//
//         _members.Add(ProjectMember.Create(userId, Id, role, invitedBy));
//         return Result.Success();
//     }
//     
//     public Result ChangeRole(UserId userId, ProjectRole newRole, UserId changedBy)
//     {
//         if (!HasPermission(changedBy, p => p.CanManageRoles()))
//         {
//             return Result.Failure(Error.Forbidden("Only managers can change roles"));
//         }
//         
//         if (HasAnyManagersBesideCurrent(userId))
//         {
//             return Result.Failure(Error.Forbidden("In project should be stay less one manager"));
//         }
//         
//         var member = _members.FirstOrDefault(m => m.UserId == userId);
//         if (member == null)
//         {
//             return Result.Failure(Error.Conflict("User not a member"));
//         }
//
//         // if (member.UserId == OwnerId)
//         // {
//         //     return Result.Failure("Cannot change owner's role");
//         // }
//
//         member.ChangeRole(newRole);
//         return Result.Success();
//     }
//     
//     public Result RemoveMember(UserId userId, UserId removedBy)
//     {
//         if (!HasPermission(removedBy, p => p.CanManageRoles()))
//         {
//             return Result.Failure(Error.Forbidden("Only managers can remove members"));
//         }
//
//         if (userId == OwnerId)
//         {
//             return Result.Failure(Error.Forbidden("Cannot remove the project owner"));
//         }
//
//         var member = _members.FirstOrDefault(m => m.UserId == userId);
//         if (member == null)
//         {
//             return Result.Failure(Error.NotFound("Member", userId));
//         }
//
//         _members.Remove(member);
//         AddDomainEvent(new VisitorRemovedDomainEvent(Id, userId));
//         return Result.Success();
//     }
//
//     public Result Archive(UserId userId)
//     {
//         if (!HasPermission(userId, p => p.CanManageRoles()))
//         {
//             return Result.Failure(Error.Forbidden("Only managers can archive projects"));
//         }
//
//         if (Status == ProjectStatus.Archived)
//         {
//             return Result.Failure(Error.Conflict("Project is already archived"));
//         }
//
//         var oldStatus = Status;
//         Status = ProjectStatus.Archived;
//         AddDomainEvent(new ProjectStatusChangedDomainEvent(Id, oldStatus, Status));
//         return Result.Success();
//     }
//
//     public Result<ProjectInvitation> CreateInvitation(ProjectRole suggestedRole, UserId createdBy)
//     {
//         if (!HasPermission(createdBy, p => p.CanManageRoles()))
//         {
//             return Result<ProjectInvitation>.Failure(Error.Forbidden("Only managers can create invitations"));
//         }
//
//         var invitation = new ProjectInvitation(suggestedRole, Settings.InvitationValidityPeriod);
//         _invitations.Add(invitation);
//         return Result<ProjectInvitation>.Success(invitation);
//     }
//     
//     public Result<ProjectRole> AcceptInvitation(string inviteCode, UserId userId)
//     {
//         var invitation = _invitations.FirstOrDefault(i => i.InviteCode == inviteCode);
//         if (invitation == null)
//         {
//             // TODO: Change to other error code
//             return Result<ProjectRole>.Failure(Error.Internal("Invalid invitation code"));
//         }
//
//         if (!invitation.IsValid())
//         {
//             return Result<ProjectRole>.Failure(Error.Validation("Invitation expired or already used"));
//         }
//
//         if (_members.Any(m => m.UserId == userId))
//         {
//             return Result<ProjectRole>.Failure(Error.Conflict("User already a member"));
//         }
//
//         invitation.MarkAsUsed();
//         var role = invitation.SuggestedRole;
//         _members.Add(ProjectMember.Create(userId, Id, role, null));
//     
//         return Result<ProjectRole>.Success(role);
//     }
//     
//     // Проверка доступа для сцены
//     public bool HasAccess(UserId userId, Func<ProjectRole, bool> permissionCheck)
//     {
//         if (Settings.IsPublic && permissionCheck(ProjectRole.Visitor))
//         {
//             return true;
//         }
//
//         var member = _members.FirstOrDefault(m => m.UserId == userId);
//         return member != null && permissionCheck(member.Role);
//     }
//     
//     public void UpdateSettings(ProjectSettings newSettings, UserId changedBy)
//     {
//         if (!HasPermission(changedBy, p => p.CanManageRoles()))
//         {
//             throw new UnauthorizedAccessException("Only managers can change project settings");
//         }
//
//         Settings = newSettings;
//     }
//     
//     private bool HasPermission(UserId userId, Func<ProjectRole, bool> permissionCheck)
//     {
//         if (userId == OwnerId)
//         {
//             return true;
//         }
//
//         var member = _members.FirstOrDefault(m => m.UserId == userId);
//         return member != null && permissionCheck(member.Role);
//     }
//
//     // TODO: check to quality
//     private bool HasAnyManagersBesideCurrent(UserId userId)
//     {
//         var otherManager = _members.FirstOrDefault(
//             m => 
//                 HasPermission(m.UserId, p => p.CanManageRoles()) &&
//                 m.UserId != userId
//         );
//         
//         return otherManager != null;
//     }
// }


// ----
//
// public class Project : AggregateRoot<ProjectId>
// {
//     private readonly List<ProjectAccess> _accessList = [];
//
//     public string Name { get; private set; }
//     public string Description { get; private set; }
//     public ProjectType Type { get; private set; }
//     public UserId OwnerId { get; private set; }
//     public ProjectStatus Status { get; private set; }
//     public DateTime CreatedAt { get; private set; }
//     public IReadOnlyCollection<ProjectAccess> AccessList => _accessList.AsReadOnly();
//     public ProjectSettings Settings { get; private set; }
//
//     private Project() { } // EF Core
//
//     private Project(ProjectId id) : base(id)
//     {
//         
//     }
//     
//     public static Result<Project> Create(
//         string name,
//         string? description,
//         ProjectType type,
//         UserId ownerId)
//     {
//         // Guard.AgainstNullOrWhiteSpace(name);
//         // Guard.AgainstLongerThan(name, 200);
//         
//         if (string.IsNullOrWhiteSpace(name))
//         {
//             throw new DomainException("Project name is required");
//         }
//         
//         if (name.Length > 200)
//         {
//             throw new DomainException("Project name must be less than 200 characters");
//         }
//     
//         var project = new Project(ProjectId.New())
//         {
//             Name = name,
//             Description = description ?? string.Empty,
//             Type = type,
//             OwnerId = ownerId,
//             Status = ProjectStatus.Draft,
//             CreatedAt = DateTime.UtcNow,
//             Settings = ProjectSettings.CreateDefault(type)
//         };
//     
//         project._accessList.Add(new ProjectAccess(ownerId, AccessLevel.RoleManager, DateTime.UtcNow));
//     
//         project.AddDomainEvent(new ProjectCreatedDomainEvent(
//             project.Id, project.Name, project.Type, project.OwnerId));
//     
//         return Result<Project>.Success(project);
//     }
//     
//     public Result AddVisitor(UserId addedByUserId, UserId visitorId)
//     {
//         if (!HasAccess(addedByUserId, AccessLevel.RoleManager))
//         {
//             return Result.Failure(Error.Forbidden("Only owner can add observers"));
//         }
//     
//         if (_accessList.Any(a => a.UserId == visitorId))
//         {
//             return Result.Failure(Error.Conflict("User already has access"));
//         }
//     
//         if (Status == ProjectStatus.Archived)
//         {
//             return Result.Failure(Error.Validation("Cannot modify archived project"));
//         }
//     
//         _accessList.Add(new ProjectAccess(visitorId, AccessLevel.Visitor, DateTime.UtcNow));
//     
//         AddDomainEvent(new VisitorAddedDomainEvent(Id, visitorId, addedByUserId));
//     
//         return Result.Success();
//     }
//     
//     public Result RemoveVisitor(UserId removedByUserId, UserId visitorId)
//     {
//         if (!HasAccess(removedByUserId, AccessLevel.RoleManager))
//         {
//             return Result.Failure(Error.Forbidden("Only owner can remove observers"));
//         }
//     
//         var access = _accessList.FirstOrDefault(a => a.UserId == visitorId);
//         if (access == null)
//         {
//             return Result.Failure(Error.NotFound("Observer", visitorId));
//         }
//     
//         if (access.AccessLevel == AccessLevel.RoleManager)
//         {
//             return Result.Failure(Error.Validation("Cannot remove owner"));
//         }
//     
//         _accessList.Remove(access);
//     
//         AddDomainEvent(new VisitorRemovedDomainEvent(Id, visitorId));
//     
//         return Result.Success();
//     }
//     
//     public Result Archive(UserId userId)
//     {
//         if (!HasAccess(userId, AccessLevel.RoleManager))
//         {
//             return Result.Failure(Error.Forbidden("Only role manager can archive"));
//         }
//     
//         Status = ProjectStatus.Archived;
//         AddDomainEvent(new ProjectStatusChangedDomainEvent(Id, ProjectStatus.Active, Status));
//     
//         return Result.Success();
//     }
//     
//     public bool HasAccess(UserId userId, AccessLevel requiredLevel)
//         => _accessList.Any(a => a.UserId == userId && a.AccessLevel >= requiredLevel);
    
    // ---------------------------------------
    
    // private readonly List<ProjectAccess> _accessList = [];
    //
    // public string Name { get; private set; }
    // public string Description { get; private set; }
    // public ProjectType Type { get; private set; }
    // public ProjectStatus Status { get; private set; }
    // public UserId OwnerId { get; private set; }
    // public ProjectSettings Settings { get; private set; }
    // public DateTime CreatedAt { get; private set; }
    // public DateTime? UpdatedAt { get; private set; }
    //
    // public IReadOnlyCollection<ProjectAccess> AccessList => _accessList;
    //
    // private Project() { } // EF Core
    //
    // public Project(ProjectId id, string name, ProjectType type, UserId ownerId, ProjectSettings settings) : base(id)
    // {
    //     Name = name;
    //     Type = type;
    //     OwnerId = ownerId;
    //     Settings = settings;
    //     Status = ProjectStatus.Draft;
    //     CreatedAt = DateTime.UtcNow;
    //
    //     // Владелец автоматически имеет полный доступ
    //     _accessList.Add(ProjectAccess.CreateOwner(ownerId, id));
    //
    //     AddDomainEvent(new ProjectCreatedDomainEvent(id, ownerId, name));
    // }
    //
    // public static Project Create(
    //     string name,
    //     string? description,
    //     ProjectType type,
    //     UserId ownerId)
    // {
    //     if (string.IsNullOrWhiteSpace(name))
    //     {
    //         throw new DomainException("Project name is required");
    //     }
    //
    //     if (name.Length > 200)
    //     {
    //         throw new DomainException("Project name must be less than 200 characters");
    //     }
    //
    //     var project = new Project
    //     {
    //         Id = new ProjectId(Guid.NewGuid()),
    //         Name = name,
    //         Description = description ?? string.Empty,
    //         Type = type,
    //         OwnerId = ownerId,
    //         Status = ProjectStatus.Draft,
    //         CreatedAt = DateTime.UtcNow,
    //         _settings = ProjectSettings.CreateDefault(type)
    //     };
    //
    //     // Владелец автоматически получает полный доступ
    //     project._accessList.Add(new ProjectAccess(
    //         ownerId, 
    //         AccessLevel.Owner, 
    //         DateTime.UtcNow));
    //
    //     project.AddDomainEvent(new ProjectCreatedDomainEvent(
    //         project.Id, 
    //         project.Name, 
    //         project.Type, 
    //         project.OwnerId));
    //
    //     return project;
    // }
    //
    // public void AddObserver(UserId observerId, IProjectAccessService accessService)
    // {
    //     if (!accessService.CanAddObserver(OwnerId, observerId))
    //     {
    //         throw new DomainException("Only owner can add observers");
    //     }
    //
    //     if (_accessList.Any(a => a.UserId == observerId))
    //     {
    //         throw new DomainException("User already has access");
    //     }
    //
    //     _accessList.Add(ProjectAccess.CreateObserver(observerId, Id));
    //
    //     AddDomainEvent(new ObserverAddedDomainEvent(Id, observerId));
    // }
    //
    // public void ChangeStatus(ProjectStatus newStatus)
    // {
    //     if (Status == ProjectStatus.Archived)
    //     {
    //         throw new DomainException("Cannot change status of archived project");
    //     }
    //
    //     Status = newStatus;
    //     UpdatedAt = DateTime.UtcNow;
    //
    //     AddDomainEvent(new ProjectStatusChangedDomainEvent(Id, newStatus));
    // }
