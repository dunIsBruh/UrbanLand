using Core.IntegrationEvents.SceneDesign;
using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Events;

namespace SceneDesign.Infrastructure.Integration.Consumers;

public class SceneCreatedDomainEventConsumer(
    IPublishEndpoint publishEndpoint,
    ILogger<SceneCreatedDomainEventConsumer> logger)
    : IConsumer<SceneCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<SceneCreatedDomainEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation("Scene {SceneId} created for project {ProjectId}",
            @event.SceneId, @event.ProjectId);

        await publishEndpoint.Publish(new SceneCreatedIntegrationEvent(
            @event.ProjectId.Value,
            @event.SceneId.Value,
            DateTime.UtcNow));
    }
}
