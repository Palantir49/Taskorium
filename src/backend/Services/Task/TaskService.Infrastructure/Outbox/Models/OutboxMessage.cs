namespace TaskService.Infrastructure.Outbox.Models;

/// <summary>
/// Каркас записи outbox-сообщения для TaskService.
/// Хранит сериализованное integration event и технические поля обработки.
/// </summary>
public class OutboxMessage
{
    /// <summary>
    /// Уникальный идентификатор outbox-записи.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Время возникновения события (UTC).
    /// </summary>
    public DateTimeOffset OccurredOnUtc { get; private set; }

    /// <summary>
    /// Тип события
    /// </summary>
    public string Type { get; private set; } = string.Empty;

    /// <summary>
    /// Сериализованный payload события.
    /// </summary>
    public string Payload { get; private set; } = string.Empty;

    /// <summary>
    /// Текущий статус обработки сообщения.
    /// </summary>
    public string Status { get; private set; } = OutboxStatuses.Pending;

    /// <summary>
    /// Количество попыток публикации.
    /// </summary>
    public int Retries { get; private set; }

    /// <summary>
    /// Время успешной обработки сообщения.
    /// </summary>
    public DateTimeOffset? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Текст последней ошибки публикации.
    /// </summary>
    public string? LastError { get; private set; }

    public OutboxMessage(
        Guid id,
        DateTimeOffset occurredOnUtc,
        string type,
        string payload,
        string status,
        int retries,
        DateTimeOffset? processedOnUtc,
        string? lastError)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Payload = payload;
        Status = status;
        Retries = retries;
        ProcessedOnUtc = processedOnUtc;
        LastError = lastError;
    }

    /// <summary>
    /// Пометить сообщение как успешно обработанное.
    /// </summary>
    public void MarkProcessed()
    {
        Status = OutboxStatuses.Processed;
        ProcessedOnUtc = DateTimeOffset.UtcNow;
        LastError = null;
    }

    /// <summary>
    /// Пометить сообщение как неуспешно обработанное и увеличить счётчик попыток.
    /// </summary>
    public void MarkFailed(string error)
    {
        Status = OutboxStatuses.Failed;
        LastError = error;
        Retries++;
    }
}


