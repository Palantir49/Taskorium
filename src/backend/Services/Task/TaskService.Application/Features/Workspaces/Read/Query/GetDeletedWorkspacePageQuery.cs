using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Workspaces.Read.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Read.Query;

public record GetDeletedWorkspacePageQuery(int Skip, int Take) : IQuery<GetDeletedWorkspacePageResult>;

