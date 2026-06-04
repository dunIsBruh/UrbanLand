using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.CreateInvitation;

public record CreateInvitationCommand(
    ProjectId ProjectId,
    ProjectRole SuggestedRole) : IRequest<Result<CreateInvitationResult>>;
