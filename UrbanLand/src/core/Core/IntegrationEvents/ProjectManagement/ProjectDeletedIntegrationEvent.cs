using Core.Abstractions;

namespace Core.IntegrationEvents.ProjectManagement;

public sealed record ProjectDeletedIntegrationEvent(Guid ProjectId) : IntegrationEvent;
