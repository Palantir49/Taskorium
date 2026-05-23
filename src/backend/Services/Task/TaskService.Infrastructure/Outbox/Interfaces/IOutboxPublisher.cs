using Taskorium.MessageBus.Abstractions;
using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Infrastructure.Outbox.Interfaces;

/// <summary>
/// Контракт публикации outbox-сообщения в message bus.
/// Используется процессором outbox как абстракция над фактической отправкой события.
/// </summary>
public interface IOutboxPublisher
{
    /// <summary>
    /// Опубликовать одно outbox-сообщение в шину.
    /// </summary>
    Task PublishAsync(OutboxMessage message, IEventBus eventBus, CancellationToken cancellationToken = default);
}
