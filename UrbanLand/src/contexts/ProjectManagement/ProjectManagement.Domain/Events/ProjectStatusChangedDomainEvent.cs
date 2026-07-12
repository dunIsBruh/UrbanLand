using Core.Abstractions;
using ProjectManagement.Domain.Enums;
using ProjectId = Core.Contracts.ProjectId;

namespace ProjectManagement.Domain.Events;

public sealed record ProjectStatusChangedDomainEvent(
    ProjectId ProjectId,
    ProjectStatus OldStatus,
    ProjectStatus NewStatus) : DomainEvent;