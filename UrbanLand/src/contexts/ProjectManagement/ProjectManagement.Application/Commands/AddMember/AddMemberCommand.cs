using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.AddMember;

public record AddMemberCommand(
    ProjectId ProjectId,
    UserId UserId,
    ProjectRole Role) : IRequest<Result>;
