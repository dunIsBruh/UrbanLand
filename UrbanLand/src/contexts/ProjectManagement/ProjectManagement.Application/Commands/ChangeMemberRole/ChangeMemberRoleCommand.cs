using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.ChangeMemberRole;

public record ChangeMemberRoleCommand(
    ProjectId ProjectId,
    UserId UserId,
    ProjectRole NewRole) : IRequest<Result>;
