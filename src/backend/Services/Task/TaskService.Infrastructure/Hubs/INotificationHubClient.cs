namespace TaskService.Infrastructure.Hubs;

/// <summary>
///     Типизированный интерфейс SignalR-клиента.
///     Методы вызываются на клиенте (браузере) из серверного кода.
/// </summary>
public interface INotificationHubClient
{
    /// <summary>Доставить уведомление конкретному пользователю.</summary>
    Task ReceiveNotification(HubNotificationMessage message);
}

/// <summary>
///     Сообщение, отправляемое клиенту через SignalR.
/// </summary>
/// <param name="EventType">
///     Тип события: "IssueCreated", "IssueAssigned", "IssueStatusChanged" и т.д.
///     Позволяет клиенту маршрутизировать уведомления.
/// </param>
/// <param name="Title">Заголовок уведомления</param>
/// <param name="Body">Текст уведомления</param>
/// <param name="Payload">Структурированные данные события (зависят от EventType)</param>
/// <param name="Timestamp">Время отправки (UTC)</param>
public record HubNotificationMessage(
    string EventType,
    string Title,
    string Body,
    object Payload,
    DateTimeOffset Timestamp);
