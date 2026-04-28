using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Write.Result;

public record CreateWorkspaceResult(Guid Id, string Name);
