using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Write.Result;

public record UpdateWorkspaceNameResult(Guid id, string name);
