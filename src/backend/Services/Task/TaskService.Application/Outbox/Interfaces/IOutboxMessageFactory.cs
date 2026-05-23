using TaskService.Infrastructure.Outbox.Models;

/// <summary>
/// Фабрика создания outbox-сообщений из integration events.
/// </summary>
namespace TaskService.Application.Outbox.Interfaces;

public interface IOutboxMessageFactory
{
    /// <summary>
    /// Создать outbox-сообщение для указанного integration event.
    /// </summary>
    OutboxMessage Create<T>(T integrationEvent, string eventType);
}
