using TaskService.Application.Features.Users.Get;
using TaskService.Application.Interfaces;

namespace TaskService.Api.Middlewares.CurrentUserContext;
/// <summary>
/// Контекст юзера, выполнившего HTTP запрос
/// </summary>
public class CurrentUserContext : ICurrentUserContext
{
    /// <summary>
    /// Юзер
    /// </summary>
    public GetUserByKeycloakIdResult? User { get; private set; }

    /// <summary>
    /// проверка инициализации юзера
    /// </summary>
    public bool IsInitialized => User is not null;

    /// <summary>
    /// Сохраняет полученного юзера, при условии что он еще не инициализирован
    /// </summary>
    /// <param name="user">контекст юзера</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Initialize(GetUserByKeycloakIdResult user)
    {
        if (IsInitialized)
            throw new InvalidOperationException("CurrentUserContext уже инициализирован для этого запроса");

        User = user ?? throw new ArgumentNullException(nameof(user));
    }
}
