using MediatR;
using ProjectManagement.Domain.Repositories;
using SharedKernel.Identity;
using SharedKernel.Primitives;

namespace ProjectManagement.Application.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Result<CreateInvitationResult>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateInvitationCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateInvitationResult>> Handle(CreateInvitationCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
            return Result<CreateInvitationResult>.Failure(Error.NotFound("Project", command.ProjectId));

        var createdBy = new UserId(_currentUserService.UserId);
        var result = project.CreateInvitation(command.SuggestedRole, createdBy);
        if (result.IsFailure)
            return Result<CreateInvitationResult>.Failure(result.Error);

        await _projectRepository.SaveAsync(project, ct);

        var invitation = result.Value;
        return Result<CreateInvitationResult>.Success(new CreateInvitationResult(
            invitation.Id.Value,
            invitation.InviteCode,
            invitation.SuggestedRole.Name,
            DateTime.UtcNow,
            invitation.ExpiresAt));
    }
}
