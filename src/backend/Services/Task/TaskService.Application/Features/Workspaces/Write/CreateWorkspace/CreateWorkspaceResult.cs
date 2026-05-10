using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

public record CreateWorkspaceResult(Guid Id, string Name);
