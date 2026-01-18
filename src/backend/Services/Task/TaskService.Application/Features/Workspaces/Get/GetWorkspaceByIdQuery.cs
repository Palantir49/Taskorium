using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Commands.Workspaces.Get;

public record class GetWorkspaceByIdQuery(Guid id) : IQuery<GetWorkspacebyIdResult>;

