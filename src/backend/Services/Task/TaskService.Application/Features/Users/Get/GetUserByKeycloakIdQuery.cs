using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Get;

public record GetUserByKeycloakIdQuery(Guid? KeycloakId) : IQuery<GetUserByKeycloakIdResult>;
