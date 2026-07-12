using Core.Identity;
using Core.Primitives;
using MediatR;
using ProjectManagement.Domain.ValueObjects;
using Core.Identity;
using Core.Primitives;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Application.Commands.RemoveMember;

public record RemoveMemberCommand(
    ProjectId ProjectId,
    UserId UserId) : IRequest<Result>;
