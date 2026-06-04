using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents.SceneDesign;

public record SceneStatisticsUpdatedIntegrationEvent(
    Guid ProjectId,
    Guid SceneId,
    int TotalObjects,
    int TotalBuildings,
    int TotalVegetation,
    DateTime UpdatedAt) : IntegrationEvent;
