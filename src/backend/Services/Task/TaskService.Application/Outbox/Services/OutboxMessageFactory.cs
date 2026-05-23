using TaskService.Application.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Application.Outbox.Services;

/// <summary>
/// Каркас фабрики создания OutboxMessage из integration events.
/// </summary>
public class OutboxMessageFactory(IOutboxSerializer serializer) : IOutboxMessageFactory
{
    /// <summary>
    /// Сериализатор payload событий.
    /// </summary>
    protected IOutboxSerializer Serializer { get; } = serializer;

    /// <summary>
    /// Создать outbox-сообщение из integration event.
    /// </summary>
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
