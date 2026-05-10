using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.WorkspaceMembers.Read.GetWorkspaceMembersHandler
{
    public record GetWorkspaceMembersQuery(Guid KeyoclakId) : IQuery<GetWorkspaceMembersResult>;
}
