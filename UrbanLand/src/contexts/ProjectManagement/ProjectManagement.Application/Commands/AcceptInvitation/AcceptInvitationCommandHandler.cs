using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<string>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public AcceptInvitationCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> Handle(AcceptInvitationCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
            return Result<string>.Failure(Error.NotFound("Project", command.ProjectId));

        var result = project.AcceptInvitation(command.InviteCode, command.UserId);
        if (result.IsFailure)
            return Result<string>.Failure(result.Error);

        await _projectRepository.SaveAsync(project, ct);

        return Result<string>.Success(result.Value.Name);
    }
}
