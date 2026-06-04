using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Commands.ChangeMemberRole;

public class ChangeMemberRoleCommandHandler : IRequestHandler<ChangeMemberRoleCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangeMemberRoleCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(ChangeMemberRoleCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var changedBy = new UserId(_currentUserService.UserId);
        return project.ChangeRole(command.UserId, command.NewRole, changedBy);
    }
}
