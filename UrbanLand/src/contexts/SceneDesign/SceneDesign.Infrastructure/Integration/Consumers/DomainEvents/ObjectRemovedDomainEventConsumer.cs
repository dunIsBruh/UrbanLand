using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Events;
using SceneDesign.Domain.Repositories;
using SharedKernel.IntegrationEvents.SceneDesign;

namespace SceneDesign.Infrastructure.Integration.Consumers.DomainEvents;

public class ObjectRemovedDomainEventConsumer(
    ISceneRepository sceneRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<ObjectRemovedDomainEventConsumer> logger)
    : IConsumer<ObjectRemovedDomainEvent>
{
    public async Task Consume(ConsumeContext<ObjectRemovedDomainEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation("Object {ObjectId} removed from scene {SceneId}",
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
