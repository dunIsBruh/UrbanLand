using ProjectManagement.Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record MemberAddedDomainEvent(
    ProjectId ProjectId,
    UserId VisitorId,
    UserId AddedByUserId) : DomainEvent;