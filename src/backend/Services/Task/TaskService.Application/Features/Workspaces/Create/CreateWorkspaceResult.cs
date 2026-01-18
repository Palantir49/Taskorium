using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Commands.Workspaces.Create;

public record CreateWorkspaceResult(Guid id, string name);
