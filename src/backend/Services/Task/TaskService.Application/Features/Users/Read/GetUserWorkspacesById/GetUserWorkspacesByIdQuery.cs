using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Read.GetUserWorkspacesById
{
    public record GetUserWorkspacesByIdQuery(Guid Id) : IQuery<GetUserWorkspacesByIdResult>;

}
