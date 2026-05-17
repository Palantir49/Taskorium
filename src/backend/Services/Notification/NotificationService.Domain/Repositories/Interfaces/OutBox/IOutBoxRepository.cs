using NotificationService.Domain.Aggregates.Outbox;

namespace NotificationService.Domain.Repositories.Interfaces.OutBox;

public interface IOutBoxRepository : IRepository<OutBoxMessage>
{
    Task<IReadOnlyList<OutBoxMessage>> AcquireBatchAsync(
        int batchSize,
        int maxRetries,
        string processorInstanceId,
        CancellationToken ct = default);
}
