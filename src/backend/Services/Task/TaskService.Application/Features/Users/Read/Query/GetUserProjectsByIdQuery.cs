using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Users.Read.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Read.Query
{
    public record GetUserProjectsByIdQuery(Guid Id,Guid WorkspaceId) : IQuery<GetUserProjectsByIdResult>;

}
