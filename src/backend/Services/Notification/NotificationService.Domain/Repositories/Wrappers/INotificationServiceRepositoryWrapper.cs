using NotificationService.Domain.Repositories.Interfaces.Notifications;

namespace NotificationService.Domain.Repositories.Wrappers;

public interface INotificationServiceRepositoryWrapper
{
    INotificationRepository NotificationRepository { get; }
}
