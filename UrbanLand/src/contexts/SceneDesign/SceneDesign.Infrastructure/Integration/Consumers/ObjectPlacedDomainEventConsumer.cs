using Core.IntegrationEvents.SceneDesign;
using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Events;
using SceneDesign.Domain.Repositories;

namespace SceneDesign.Infrastructure.Integration.Consumers;

public class ObjectPlacedDomainEventConsumer(
    ISceneRepository sceneRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<ObjectPlacedDomainEventConsumer> logger)
    : IConsumer<ObjectPlacedDomainEvent>
{
    public async Task Consume(ConsumeContext<ObjectPlacedDomainEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation("Object {ObjectId} placed in scene {SceneId}",
            @event.ObjectId, @event.SceneId);

        var scene = await sceneRepository.GetByIdAsync(@event.SceneId);
        if (scene == null) return;

        await publishEndpoint.Publish(new SceneStatisticsUpdatedIntegrationEvent(
            scene.ProjectId.Value,
            scene.Id.Value,
            scene.Objects.Count,
            0,
            0,
            DateTime.UtcNow));
    }
}
