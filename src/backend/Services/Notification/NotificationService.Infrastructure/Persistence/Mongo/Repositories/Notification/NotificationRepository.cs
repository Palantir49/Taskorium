using MongoDB.Driver;
using NotificationService.Domain.Repositories.Interfaces.Notifications;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.Abstracts;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.Notification;

public class NotificationRepository(NotificationDbContext context)
    : RepositoryBase<Domain.Aggregates.Notification.Notification>(context), INotificationRepository
{
    public async Task<Domain.Aggregates.Notification.Notification?> GetByIdempotencyKeyAsync(Guid id)
    {
        var filter = Builders<Domain.Aggregates.Notification.Notification>.Filter
            .Eq("EventIdempotencyKey", id.ToString());


        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
