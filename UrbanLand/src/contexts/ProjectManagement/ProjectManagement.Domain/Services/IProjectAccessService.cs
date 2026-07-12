using Core.Identity;
using ProjectManagement.Domain.ValueObjects;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Services;

public interface IProjectAccessService
{
    Task<bool> CanUserAccessProject(UserId userId, ProjectId projectId, Func<ProjectRole, bool> requiredPermission);
}