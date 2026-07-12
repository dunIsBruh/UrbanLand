using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.Services;
using ProjectManagement.Domain.ValueObjects;
using Core.Identity;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Infrastructure.Services;

public class ProjectAccessService(IProjectRepository projectRepository) : IProjectAccessService
{
    public async Task<bool> CanUserAccessProject(
        UserId userId, 
        ProjectId projectId, 
        Func<ProjectRole, bool> requiredPermission)
    {
        var project = await projectRepository.GetByIdAsync(projectId);
        
        if (project == null)
        {
            return false;
        }

        return project.HasAccess(userId, requiredPermission);
    }
}