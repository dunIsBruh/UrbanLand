using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Identity;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.AcceptInvitation;

public record AcceptInvitationCommand(
    ProjectId ProjectId,
    string InviteCode,
    UserId UserId) : IRequest<Result<string>>;
