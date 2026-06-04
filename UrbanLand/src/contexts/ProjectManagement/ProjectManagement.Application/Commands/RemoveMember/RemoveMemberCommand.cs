using MediatR;
using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Identity;
using SharedKernel.Primitives;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.RemoveMember;

public record RemoveMemberCommand(
    ProjectId ProjectId,
    UserId UserId) : IRequest<Result>;
