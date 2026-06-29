using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public AddMemberCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(AddMemberCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var invitedBy = new UserId(_currentUserAccessor.UserId);
        return project.AddMember(command.UserId, command.Role, invitedBy);
    }
}
