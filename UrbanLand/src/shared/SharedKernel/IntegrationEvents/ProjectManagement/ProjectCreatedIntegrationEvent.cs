using SharedKernel.Abstractions;

namespace SharedKernel.IntegrationEvents;

public sealed record ProjectCreatedIntegrationEvent(
    Guid ProjectId, 
    string ProjectName, 
    Guid OwnerId, 
    string SceneType, 
    DateTime CreatedAt
    ) : IntegrationEvent;
