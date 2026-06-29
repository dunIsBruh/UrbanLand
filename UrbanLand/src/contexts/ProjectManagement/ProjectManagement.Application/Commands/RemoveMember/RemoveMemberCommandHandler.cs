using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.RemoveMember;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public RemoveMemberCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(RemoveMemberCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var removedBy = new UserId(_currentUserAccessor.UserId);
        return project.RemoveMember(command.UserId, removedBy);
    }
}
