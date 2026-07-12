using Core.Abstractions;

namespace Core.IntegrationEvents.ProjectManagement;

public sealed record ProjectCreatedIntegrationEvent(
    Guid ProjectId, 
    string ProjectName, 
    Guid OwnerId, 
    string SceneType, 
    DateTime CreatedAt
    ) : IntegrationEvent;
