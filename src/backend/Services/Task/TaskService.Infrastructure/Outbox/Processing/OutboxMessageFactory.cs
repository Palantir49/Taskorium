using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Infrastructure.Outbox.Processing;

/// <summary>
/// Фабрика создания OutboxMessage из integration events.
/// </summary>
public class OutboxMessageFactory(IOutboxSerializer serializer) : IOutboxMessageFactory
{
    protected IOutboxSerializer Serializer { get; } = serializer;

    public OutboxMessage Create<T>(T integrationEvent, string eventType)
    {
        var payload = Serializer.Serialize(integrationEvent);
        return new OutboxMessage(
            Guid.CreateVersion7(),
            DateTimeOffset.UtcNow,
            eventType,
            payload,
            OutboxStatuses.Pending,
            0,
            null,
            null
        );
    }
}
