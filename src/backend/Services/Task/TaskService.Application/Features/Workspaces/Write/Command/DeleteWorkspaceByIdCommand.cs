using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Workspaces.Write.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.Command;

public record DeleteWorkspaceByIdCommand(Guid Id) : ICommand<DeleteWorkspaceByIdResult>;
