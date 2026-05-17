using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspacePage;

public class GetWorkspacePageHandler(TaskServiceDbContext context) : IRequestHandler<GetWorkspacePageQuery, GetWorkspacePageResult>
{
    public async Task<GetWorkspacePageResult> Handle(GetWorkspacePageQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
        {
            return new GetWorkspacePageResult(workspaces: []);
        }

        var result = await context.Workspaces
            .AsNoTracking()
            .Where(workspace => workspace.WorkspaceMembers.Any(member => member.UserId == request.UserId.Value))
            .OrderByDescending(workspace => workspace.CreatedDate)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new WorkspaceResponse(id: x.Id,
                                               name: x.Name.Value,
                                               createdDate: x.CreatedDate))
            .ToListAsync(cancellationToken);

        return new GetWorkspacePageResult(workspaces: result);
    }
}
