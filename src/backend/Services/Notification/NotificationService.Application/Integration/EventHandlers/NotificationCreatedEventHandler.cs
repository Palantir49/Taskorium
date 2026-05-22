using Mediator;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Mapping;
using Taskorium.IntegrationEvents.Notifications;
using Taskorium.MessageBus.Abstractions;

namespace NotificationService.Application.Integration.EventHandlers;

public class NotificationCreatedEventHandler(
    ILogger<NotificationCreatedEventHandler> logger,
    NotificationMapper notificationMapper,
    IMediator mediator)
    : IEventHandler<NotificationCreatedIntegrationEvent>
{
    public async Task Handle(NotificationCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Принято событие типа {EventType}. Ключ идемпотентности: {IdempotencyKey}",
            typeof(NotificationCreatedEventHandler), @event.Id);

        var command = notificationMapper.MapToCreateNotificationCommand(@event);

        var result = await mediator.Send(command, cancellationToken);
        logger.LogInformation("Событие {IdempotencyKey} обработано. Notification id: {NotificationId}", @event.Id,
            result.NotificationId);
    }
}
