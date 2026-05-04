using TaskService.Application.Features.Users.Read.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Read.Query;

public record GetUserByKeycloakIdQuery(Guid? KeycloakId) : IQuery<GetUserByKeycloakIdResult>;
