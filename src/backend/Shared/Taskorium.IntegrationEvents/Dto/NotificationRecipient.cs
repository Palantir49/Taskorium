namespace Taskorium.IntegrationEvents.Dto;

/// <summary>
///     Получатель уведомления.
///     Содержит ВСЕ данные, необходимые для отправки.
///     NotificationService НЕ ЗНАЕТ пользователей, поэтому все данные здесь.
/// </summary>
public record NotificationRecipient
{
    /// <summary>
    ///     Идентификатор пользователя в системе
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    ///     Полное имя (для персонализации)
    /// </summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>
    ///     Email адрес (если есть)
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    ///     Номер телефона (если есть)
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    ///     Каналы, через которые нужно отправить уведомление
    ///     Пустой список = отправить через все доступные каналы
    /// </summary>
    public List<string> PreferredChannels { get; init; } = [];

    /// <summary>
    ///     Заглушен ли пользователь (не отправлять уведомления)
    /// </summary>
    public bool IsMuted { get; init; } = false;
}
