using Core.Abstractions;

namespace Core.IntegrationEvents.ProjectManagement;

public sealed record ProjectArchivedIntegrationEvent(Guid ProjectId) : IntegrationEvent;
