using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Events;

namespace SceneDesign.Infrastructure.Integration.Consumers.DomainEvents;

public class ObjectMovedDomainEventConsumer(
    ILogger<ObjectMovedDomainEventConsumer> logger)
    : IConsumer<ObjectMovedDomainEvent>
{
    public Task Consume(ConsumeContext<ObjectMovedDomainEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation("Object {ObjectId} moved in scene {SceneId}: ({OldPos}) -> ({NewPos})",
            @event.ObjectId, @event.SceneId,
            @event.OldPosition, @event.NewPosition);

        return Task.CompletedTask;
    }
}
