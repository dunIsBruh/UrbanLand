using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.CreateInvitation;

public record CreateInvitationCommand(
    ProjectId ProjectId,
    ProjectRole SuggestedRole) : IRequest<Result<CreateInvitationResult>>;
