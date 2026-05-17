using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.WorkspaceMembers.Read.GetWorkspaceMembersHandler
{
    public record GetWorkspaceMembersByKeycloakIdQuery(Guid KeyoclakId) : IQuery<GetWorkspaceMembersByKeycloakIdResult>;
}
