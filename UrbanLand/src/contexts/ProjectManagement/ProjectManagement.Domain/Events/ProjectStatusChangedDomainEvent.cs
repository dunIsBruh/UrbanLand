using ProjectManagement.Domain.Enums;
using SharedKernel.Abstractions;
using ProjectId = SharedKernel.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record ProjectStatusChangedDomainEvent(
    ProjectId ProjectId,
    ProjectStatus OldStatus,
    ProjectStatus NewStatus) : DomainEvent;