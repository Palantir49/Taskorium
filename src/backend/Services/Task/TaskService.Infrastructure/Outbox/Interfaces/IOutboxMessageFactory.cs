using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Infrastructure.Outbox.Interfaces;

/// <summary>
/// Фабрика создания outbox-сообщений из integration events.
/// </summary>
public interface IOutboxMessageFactory
{
    /// <summary>
    /// Создать outbox-сообщение для указанного integration event.
    /// </summary>
    OutboxMessage Create<T>(T integrationEvent, string eventType);
}
