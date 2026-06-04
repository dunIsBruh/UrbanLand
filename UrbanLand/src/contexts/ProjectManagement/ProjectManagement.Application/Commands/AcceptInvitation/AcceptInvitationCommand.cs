using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.AcceptInvitation;

public record AcceptInvitationCommand(
    ProjectId ProjectId,
    string InviteCode,
    UserId UserId) : IRequest<Result<string>>;
