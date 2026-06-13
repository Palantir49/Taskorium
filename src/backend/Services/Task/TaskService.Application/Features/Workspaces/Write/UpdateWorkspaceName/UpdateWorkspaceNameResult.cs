using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Write.UpdateWorkspaceName;

public record UpdateWorkspaceNameResult(Guid Id, string Name);
