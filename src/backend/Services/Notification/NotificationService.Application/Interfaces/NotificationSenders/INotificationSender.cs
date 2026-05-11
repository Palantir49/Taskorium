using NotificationService.Domain.Aggregates.Notification;

namespace NotificationService.Application.Interfaces.NotificationSenders;

public interface INotificationSender
{
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}
