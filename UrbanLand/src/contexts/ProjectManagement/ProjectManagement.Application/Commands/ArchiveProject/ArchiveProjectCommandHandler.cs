using Core.Identity;
using Core.Primitives;
using MassTransit;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.IntegrationEvents;
using Core.IntegrationEvents.ProjectManagement;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.ArchiveProject;

public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public ArchiveProjectCommandHandler(
        IProjectRepository projectRepository,
        IPublishEndpoint publishEndpoint,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(ArchiveProjectCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var userId = new UserId(_currentUserAccessor.UserId);
        var result = project.Archive(userId);
        if (result.IsFailure)
        {
            return result;
        }

        await _projectRepository.SaveAsync(project, ct);

        await _publishEndpoint.Publish(
            new ProjectArchivedIntegrationEvent(command.ProjectId.Value), ct);

        return Result.Success();
    }
}
