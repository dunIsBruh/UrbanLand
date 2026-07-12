using Core.Abstractions;

namespace Core.IntegrationEvents.ProjectManagement;

public sealed record ProjectStatusChangedIntegrationEvent(
    Guid ProjectId,
    DateTime ChangeAt,
    string NewStatus,
    string? OldStatus = null
    ) : IntegrationEvent;