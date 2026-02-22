using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Users.Get;

public class GetWorkspacePageHandler(TaskServiceDbContext context) : IRequestHandler<GetWorkspacePageQuery, GetWorkspacePageResult>
{
    public async Task<GetWorkspacePageResult> Handle(GetWorkspacePageQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Workspaces
            .AsNoTracking()
            .Skip(request.skip)
            .Take(request.take)
            .Select(x => new WorkspaceResponse(id: x.Id,
                                               name: x.Name.Value,
                                               createdDate: x.CreatedDate,
                                               ownerId: x.OwnerId))
            .ToListAsync();

        return new GetWorkspacePageResult(workspaces: result);
    }
}
