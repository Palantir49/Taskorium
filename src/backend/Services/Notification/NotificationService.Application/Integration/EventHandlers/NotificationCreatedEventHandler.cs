using Taskorium.IntegrationEvents.Notifications;
using Taskorium.MessageBus.Abstractions;

namespace NotificationService.Application.Integration.EventHandlers;

public class NotificationCreatedEventHandler : IEventHandler<NotificationCreatedIntegrationEvent>
{
    public Task Handle(NotificationCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
