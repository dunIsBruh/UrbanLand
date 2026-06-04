using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents.SceneDesign;

public sealed record SceneCreatedIntegrationEvent(
    Guid ProjectId,
    Guid SceneId,
    DateTime CreatedAt) : IntegrationEvent;
