using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Commands.RemoveMember;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveMemberCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(RemoveMemberCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var removedBy = new UserId(_currentUserService.UserId);
        return project.RemoveMember(command.UserId, removedBy);
    }
}
