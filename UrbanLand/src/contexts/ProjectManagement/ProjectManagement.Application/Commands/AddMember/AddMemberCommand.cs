using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.AddMember;

public record AddMemberCommand(
    ProjectId ProjectId,
    UserId UserId,
    ProjectRole Role) : IRequest<Result>;
