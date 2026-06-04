using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents;

public sealed record ProjectArchivedIntegrationEvent(Guid ProjectId) : IntegrationEvent;
