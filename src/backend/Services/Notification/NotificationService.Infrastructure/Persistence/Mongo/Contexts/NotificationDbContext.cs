using MongoDB.Driver;
using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Aggregates.Outbox;

namespace NotificationService.Infrastructure.Persistence.Mongo.Contexts;

public class NotificationDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("NotificationDb");

    public IMongoCollection<Notification> Notifications =>
        _database.GetCollection<Notification>("Notifications");

    public IMongoCollection<OutBoxMessage> OutBoxMessages =>
        _database.GetCollection<OutBoxMessage>("OutBoxMessages");

    internal IClientSessionHandle? CurrentSession { get; set; }

    public async Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default)
    {
        return await mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
    }

    public async Task EnsureNotificationIndexesAsync(CancellationToken ct)
    {
        var idempotencyIndex = new CreateIndexModel<Notification>(
            Builders<Notification>.IndexKeys
                .Ascending("EventIdempotencyKey"),
            new CreateIndexOptions { Unique = true, Name = "idx_idempotency_key_unique" });

        await Notifications.Indexes
            .CreateOneAsync(idempotencyIndex, cancellationToken: ct);
    }
}
