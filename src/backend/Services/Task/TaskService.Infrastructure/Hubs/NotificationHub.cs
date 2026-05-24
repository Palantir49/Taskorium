using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskService.Infrastructure.Hubs;

/// <summary>
///     SignalR-хаб уведомлений.
///     Аутентификация — JWT Bearer (токен передаётся через query string ?access_token=...).
///     Маршрутизация к конкретному пользователю реализована через IHubContext.Clients.User(keycloakId),
///     где keycloakId = claim NameIdentifier (sub) из JWT.
/// </summary>
[Authorize]
public class NotificationHub : Hub<INotificationHubClient>
{
}
