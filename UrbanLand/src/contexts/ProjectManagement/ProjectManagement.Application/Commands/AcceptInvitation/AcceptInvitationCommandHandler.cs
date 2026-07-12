using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<string>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public AcceptInvitationCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<string>> Handle(AcceptInvitationCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
        {
            return Result<string>.Failure(Error.NotFound("Project", command.ProjectId));
        }

        var result = project.AcceptInvitation(command.InviteCode, command.UserId);
        if (result.IsFailure)
        {
            return Result<string>.Failure(result.Error);
        }

        await _projectRepository.SaveAsync(project, ct);

        return Result<string>.Success(result.Value.Name);
    }
}
