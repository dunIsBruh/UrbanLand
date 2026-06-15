using MassTransit;
using MediatR;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Exceptions;
using SharedKernel.Identity;
using SharedKernel.IntegrationEvents;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectId>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IPublishEndpoint publishEndpoint,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _publishEndpoint = publishEndpoint;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<ProjectId>> Handle(CreateProjectCommand command, CancellationToken ct)
    {
        var ownerId = new UserId(_currentUserAccessor.UserId);

        Result<Project> result;
        try
        {
            result = Project.Create(command.Name, ownerId, command.Description);
        }
        catch (DomainException ex)
        {
            return Result<ProjectId>.Failure(Error.Validation(ex.Message));
        }

        if (result.IsFailure)
        {
            return Result<ProjectId>.Failure(result.Error);
        }

        var project = result.Value;
        await _projectRepository.SaveAsync(project, ct);

        await _publishEndpoint.Publish(
            new ProjectCreatedIntegrationEvent(project.Id.Value, project.Name, project.OwnerId.Value, command.SceneType, project.CreatedAt), 
            ct);

        return Result<ProjectId>.Success(project.Id);
    }
}
