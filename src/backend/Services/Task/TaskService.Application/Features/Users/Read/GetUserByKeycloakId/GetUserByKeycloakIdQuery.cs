using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Read.GetUserByKeycloakId;

public record GetUserByKeycloakIdQuery(Guid? KeycloakId) : IQuery<GetUserByKeycloakIdResult>;
