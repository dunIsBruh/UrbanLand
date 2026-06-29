using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Identity;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.ChangeMemberRole;

public record ChangeMemberRoleCommand(
    ProjectId ProjectId,
    UserId UserId,
    ProjectRole NewRole) : IRequest<Result>;
