using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.DeleteWorkspaceById;

public record DeleteWorkspaceByIdCommand(Guid Id) : ICommand<DeleteWorkspaceByIdResult>;
