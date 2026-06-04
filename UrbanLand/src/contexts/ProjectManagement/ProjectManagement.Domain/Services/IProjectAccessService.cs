using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Services;

public interface IProjectAccessService
{
    Task<bool> CanUserAccessProject(UserId userId, ProjectId projectId, Func<ProjectRole, bool> requiredPermission);
}