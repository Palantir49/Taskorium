using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Workspaces.Write.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.Command;

public record CreateWorkspaceCommand(string Name, Guid OwnerId) : ICommand<CreateWorkspaceResult>;
