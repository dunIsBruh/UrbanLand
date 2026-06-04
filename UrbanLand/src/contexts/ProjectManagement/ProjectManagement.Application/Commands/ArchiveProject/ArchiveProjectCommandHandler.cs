using MassTransit;
using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.IntegrationEvents;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Commands.ArchiveProject;

public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserService _currentUserService;

    public ArchiveProjectCommandHandler(
        IProjectRepository projectRepository,
        IPublishEndpoint publishEndpoint,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(ArchiveProjectCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var userId = new UserId(_currentUserService.UserId);
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
