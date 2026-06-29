using Core.Identity;
using Core.Primitives;
using MassTransit;
using MediatR;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.IntegrationEvents;
using Core.IntegrationEvents.ProjectManagement;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository,
        IPublishEndpoint publishEndpoint,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(DeleteProjectCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var userId = new UserId(_currentUserAccessor.UserId);
        if (userId != project.OwnerId && !project.HasAccess(userId, p => p.CanManageRoles()))
        {
            return Result.Failure(Error.Forbidden("Only the owner or a manager can delete the project"));
        }

        await _projectRepository.DeleteAsync(project, ct);

        await _publishEndpoint.Publish(
            new ProjectDeletedIntegrationEvent(command.ProjectId.Value), ct);

        return Result.Success();
    }
}
