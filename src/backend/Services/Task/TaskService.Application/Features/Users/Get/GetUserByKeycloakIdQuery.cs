using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Get
{
    public record GetUserByKeycloakIdQuery(Guid keycloakId) : IQuery<GetUserByKeycloakIdResult>;
}
