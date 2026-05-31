namespace TaskService.Infrastructure.Outbox.Options;

/// <summary>
/// Настройки каркаса Outbox-процессора.
/// </summary>
public class OutboxOptions
{
    /// <summary>
    /// Максимальный размер одной порции сообщений.
    /// </summary>
    public int BatchSize { get; set; } = 50;

    /// <summary>
    /// Максимально допустимое количество попыток публикации.
    /// </summary>
    public int MaxRetries { get; set; } = 5;

    /// <summary>
    /// Интервал опроса outbox в секундах.
    /// </summary>
    public int PollingIntervalSeconds { get; set; } = 30;
}
