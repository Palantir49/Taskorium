using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Workspaces.Update;

public record  UpdateWorkspaceNameResult(Guid id,string name);
