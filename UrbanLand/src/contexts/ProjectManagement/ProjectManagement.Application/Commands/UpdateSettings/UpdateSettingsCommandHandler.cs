using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.UpdateSettings;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public UpdateSettingsCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(UpdateSettingsCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
            return Result.Failure(Error.NotFound("Project", command.ProjectId));

        var userId = new UserId(_currentUserAccessor.UserId);

        return Result.Success();
    }
}
