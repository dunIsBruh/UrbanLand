using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.Repositories;
using Core.Identity;
using Core.Primitives;

namespace ProjectManagement.Application.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Result<CreateInvitationResult>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CreateInvitationCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        _projectRepository = projectRepository;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<CreateInvitationResult>> Handle(CreateInvitationCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
            return Result<CreateInvitationResult>.Failure(Error.NotFound("Project", command.ProjectId));

        var createdBy = new UserId(_currentUserAccessor.UserId);
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
