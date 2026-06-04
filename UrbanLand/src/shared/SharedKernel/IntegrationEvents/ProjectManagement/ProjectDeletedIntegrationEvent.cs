using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents;

public sealed record ProjectDeletedIntegrationEvent(Guid ProjectId) : IntegrationEvent;
