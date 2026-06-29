using SharedKernel.Abstractions;
using SharedKernel.Identity;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record MemberRemovedDomainEvent(ProjectId ProjectId, UserId ObserverId) : DomainEvent;