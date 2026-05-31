namespace TaskService.Infrastructure.Outbox.Interfaces;

/// <summary>
/// Контракт обработчика outbox-пакета.
/// Отделяет бизнес-логику обработки outbox от механики запуска в hosted service.
/// </summary>
public interface IOutboxProcessor
{
    /// <summary>
    /// Обработать одну порцию outbox-сообщений.
    /// </summary>
    Task ProcessBatchAsync(CancellationToken cancellationToken = default);
}
