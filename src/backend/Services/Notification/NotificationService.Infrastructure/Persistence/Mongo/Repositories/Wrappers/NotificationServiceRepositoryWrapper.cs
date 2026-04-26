using NotificationService.Domain.Repositories.Interfaces.Notifications;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.Notification;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.Wrappers;

public class NotificationServiceRepositoryWrapper(NotificationDbContext notificationDbContext)
    : INotificationServiceRepositoryWrapper
{
    public INotificationRepository NotificationRepository
    {
        get
        {
            field ??= new NotificationRepository(notificationDbContext);
            return field;
        }
    }
}
