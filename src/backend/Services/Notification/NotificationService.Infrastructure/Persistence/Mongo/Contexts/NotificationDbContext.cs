using MongoDB.Driver;
using NotificationService.Domain.Aggregates.Notification;

namespace NotificationService.Infrastructure.Persistence.Mongo.Contexts;

public class NotificationDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("NotificationDb");

    public IMongoCollection<Notification> Notifications =>
        _database.GetCollection<Notification>("Notifications");
}
