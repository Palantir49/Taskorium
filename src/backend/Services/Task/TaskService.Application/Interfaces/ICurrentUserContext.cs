using TaskService.Application.Features.Users.Read.Result;

namespace TaskService.Application.Interfaces;

public interface ICurrentUserContext
{
    GetUserByKeycloakIdResult User { get; }
    bool IsInitialized { get; }
    void Initialize(GetUserByKeycloakIdResult user);
}
