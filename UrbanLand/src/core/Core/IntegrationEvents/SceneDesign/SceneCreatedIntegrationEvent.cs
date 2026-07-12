using Core.Abstractions;

namespace Core.IntegrationEvents.SceneDesign;

public sealed record SceneCreatedIntegrationEvent(
    Guid ProjectId,
    Guid SceneId,
    DateTime CreatedAt) : IntegrationEvent;
