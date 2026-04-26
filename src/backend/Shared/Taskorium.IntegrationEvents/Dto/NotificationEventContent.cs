namespace Taskorium.IntegrationEvents.Dto;

/// <summary>
///     Контент уведомления
/// </summary>
public record NotificationEventContent
{
    /// <summary>
    ///     Тема сообщения
    /// </summary>
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    ///     Тело сообщения
    ///     Может быть HTML для Email или plain text для SMS
    /// </summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>
    ///     Ссылка для действия (перейти к задаче/проекту)
    /// </summary>
    public string? ActionUrl { get; init; }

    /// <summary>
    ///     Дополнительные метаданные
    /// </summary>
    public Dictionary<string, string> Metadata { get; init; } = [];
}
