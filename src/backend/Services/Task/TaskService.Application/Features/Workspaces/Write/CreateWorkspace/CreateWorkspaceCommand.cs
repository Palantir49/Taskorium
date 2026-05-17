using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

public record CreateWorkspaceCommand(string Name, Guid OwnerId) : ICommand<CreateWorkspaceResult>;
