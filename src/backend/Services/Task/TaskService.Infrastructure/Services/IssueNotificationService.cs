using Microsoft.AspNetCore.SignalR;
using TaskService.Infrastructure.Hubs;

namespace TaskService.Infrastructure.Services;

/// <summary>
///     Отправляет уведомления конкретным пользователям по их Keycloak ID (claim sub).
/// </summary>
public sealed class IssueNotificationService(
    IHubContext<NotificationHub, INotificationHubClient> hubContext)
{
    public async Task NotifyIssueCreatedAsync(
        Guid issueId,
        string issueName,
        string issueKey,
        Guid projectId,
        string projectName,
        IEnumerable<string> recipientKeycloakIds,
        CancellationToken cancellationToken = default)
    {
        var payload = new IssueCreatedPayload(
            issueId,
            issueKey,
            issueName,
            projectId,
            projectName);

        var message = new HubNotificationMessage(
            NotificationEventTypes.IssueCreated,
            "Новая задача",
            $"Вам назначена задача «{issueName}» в проекте «{projectName}»",
            payload,
            DateTimeOffset.UtcNow);

        // Отправляем каждому получателю параллельно.
        // Clients.User() использует claim NameIdentifier (sub из JWT = KeycloakId).
        // Если пользователь не подключён — сообщение молча игнорируется SignalR.
        var sendTasks = recipientKeycloakIds
            .Select(id => hubContext.Clients.User(id).ReceiveNotification(message));

        await Task.WhenAll(sendTasks);
    }
}

/// <summary>Данные события создания задачи.</summary>
public record IssueCreatedPayload(
    Guid IssueId,
    string IssueKey,
    string IssueName,
    Guid ProjectId,
    string ProjectName);

/// <summary>
///     Константы типов событий SignalR.
///     Используются клиентом для маршрутизации обработчиков.
/// </summary>
public static class NotificationEventTypes
{
    public const string IssueCreated = "IssueCreated";
    // Будущие события:
    // public const string IssueAssigned    = "IssueAssigned";
    // public const string IssueStatusChanged = "IssueStatusChanged";
    // public const string CommentAdded     = "CommentAdded";
}
