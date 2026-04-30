using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Workspaces.Read.Query;
using TaskService.Application.Features.Workspaces.Read.Result;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Workspaces.Read.Handler;

public class GetWorkspacePageHandler(TaskServiceDbContext context) : IRequestHandler<GetWorkspacePageQuery, GetWorkspacePageResult>
{
    public async Task<GetWorkspacePageResult> Handle(GetWorkspacePageQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Workspaces
            .AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new WorkspaceResponse(id: x.Id,
                                               name: x.Name.Value,
                                               createdDate: x.CreatedDate))
            .ToListAsync();

        return new GetWorkspacePageResult(workspaces: result);
    }
}
