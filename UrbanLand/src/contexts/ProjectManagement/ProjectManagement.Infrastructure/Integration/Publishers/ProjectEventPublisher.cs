using MassTransit;
using Core.IntegrationEvents;
using Core.IntegrationEvents.ProjectManagement;

namespace ProjectManagement.Infrastructure.Integration.Publishers;

public class ProjectEventPublisher(IPublishEndpoint publishEndpoint)
{
    public async Task PublishProjectCreatedAsync(
        Guid projectId, 
        string name, 
        string type, 
        Guid ownerId, 
        CancellationToken ct = default)
    {
        await publishEndpoint.Publish(
            new ProjectCreatedIntegrationEvent
            (
                projectId,
                name,
                ownerId,
                type,
                DateTime.UtcNow
            ), 
            ct);
    }

    public async Task PublishProjectStatusChangedAsync(
        Guid projectId, 
        string newStatus, 
        string? oldStatus = null,
        CancellationToken ct = default)
    {
        await publishEndpoint.Publish(
            new ProjectStatusChangedIntegrationEvent
            (
                projectId,
                DateTime.UtcNow,
                newStatus,
                oldStatus
            ), 
            ct);
    }
}