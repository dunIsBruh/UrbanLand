using MassTransit;
using Microsoft.Extensions.Logging;
using SceneDesign.Domain.Events;

namespace SceneDesign.Infrastructure.Integration.Consumers.DomainEvents;

public class ViewModeChangedDomainEventConsumer(
    ILogger<ViewModeChangedDomainEventConsumer> logger)
    : IConsumer<ViewModeChangedDomainEvent>
{
    public Task Consume(ConsumeContext<ViewModeChangedDomainEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation("Scene {SceneId} view mode changed: {OldMode} -> {NewMode}",
            @event.SceneId, @event.OldMode, @event.NewMode);

        return Task.CompletedTask;
    }
}
