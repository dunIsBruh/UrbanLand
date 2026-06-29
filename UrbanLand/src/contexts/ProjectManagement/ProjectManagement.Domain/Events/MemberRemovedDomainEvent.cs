using Core.Abstractions;
using Core.Identity;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record MemberRemovedDomainEvent(ProjectId ProjectId, UserId ObserverId) : DomainEvent;