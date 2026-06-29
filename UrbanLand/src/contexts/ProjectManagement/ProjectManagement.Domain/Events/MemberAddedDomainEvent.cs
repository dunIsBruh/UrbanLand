using Core.Abstractions;
using Core.Identity;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record MemberAddedDomainEvent(
    ProjectId ProjectId,
    UserId VisitorId,
    UserId AddedByUserId) : DomainEvent;