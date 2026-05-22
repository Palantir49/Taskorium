using MongoDB.Driver;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Repositories.Interfaces.OutBox;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.Abstracts;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.OutBox;

public class OutboxRepository(NotificationDbContext context)
    : RepositoryBase<OutBoxMessage>(context), IOutBoxRepository
{
    private static readonly TimeSpan LockTimeout = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     Атомарно выбирает И блокирует батч сообщений.
    ///     Намеренно НЕ использует context.CurrentSession (транзакцию):
    ///     транзакция изолирует изменения — другие обработчики не увидят блокировку
    ///     до Commit, что полностью убивает смысл локинга.
    ///     Каждый FindOneAndUpdate без транзакции виден всем сразу — это то что нужно.
    /// </summary>
    public async Task<IReadOnlyList<OutBoxMessage>> AcquireBatchAsync(
        int batchSize,
        int maxRetries,
        string processorInstanceId,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var lockExpiry = now - LockTimeout;
        var result = new List<OutBoxMessage>(batchSize);

        // Фильтр строится один раз — одинаков для всех итераций
        var filter = Builders<OutBoxMessage>.Filter.And(
            Builders<OutBoxMessage>.Filter
                .Eq(m => m.Processed, false),
            Builders<OutBoxMessage>.Filter
                .Lt(m => m.RetryCount, maxRetries),
            Builders<OutBoxMessage>.Filter.Or(
                Builders<OutBoxMessage>.Filter
                    .Eq(m => m.LockedAt, null),
                Builders<OutBoxMessage>.Filter
                    .Lt(m => m.LockedAt, lockExpiry)));

        var update = Builders<OutBoxMessage>.Update
            .Set(m => m.LockedAt, now)
            .Set(m => m.LockedBy, processorInstanceId);

        var options = new FindOneAndUpdateOptions<OutBoxMessage>
        {
            ReturnDocument = ReturnDocument.After,
            Sort = Builders<OutBoxMessage>.Sort
                .Ascending(m => m.CreatedAt)
        };


        for (var i = 0; i < batchSize; i++)
        {
            var message = await Collection
                .FindOneAndUpdateAsync(filter, update, options, ct);

            if (message is null)
            {
                break;
            }

            result.Add(message);
        }

        return result;
    }

    public async Task<IReadOnlyList<OutBoxMessage>> GetPendingAsync(
        int batchSize,
        int maxRetries,
        CancellationToken ct = default)
    {
        var filter = Builders<OutBoxMessage>.Filter.And(
            Builders<OutBoxMessage>.Filter
                .Eq(m => m.Processed, false),
            Builders<OutBoxMessage>.Filter
                .Lt(m => m.RetryCount, maxRetries));

        return await Collection
            .Find(filter)
            .Sort(Builders<OutBoxMessage>.Sort.Ascending(m => m.CreatedAt))
            .Limit(batchSize)
            .ToListAsync(ct);
    }
}
