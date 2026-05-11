using NotificationService.Application.Features.Notification.CreateNotification.Command;
using Riok.Mapperly.Abstractions;
using Taskorium.IntegrationEvents.Notifications;

namespace NotificationService.Application.Mapping;

[Mapper]
public partial class NotificationMapper
{
    [MapProperty(nameof(NotificationCreatedIntegrationEvent.Id), nameof(CreateNotificationCommand.IdempotencyKey))]
    public partial CreateNotificationCommand MapToCreateNotificationCommand(
        NotificationCreatedIntegrationEvent @event);
}
