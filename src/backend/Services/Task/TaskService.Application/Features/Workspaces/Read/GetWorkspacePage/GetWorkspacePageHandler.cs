using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspacePage;

public class GetWorkspacePageHandler(TaskServiceDbContext context)
    : IRequestHandler<GetWorkspacePageQuery, GetWorkspacePageResult>
{
    public async Task<GetWorkspacePageResult> Handle(GetWorkspacePageQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
        {
            return new GetWorkspacePageResult([]);
        }

        var result = await context.Workspaces
            .AsNoTracking()
            .Where(workspace => workspace.WorkspaceMembers.Any(member => member.UserId == request.UserId.Value))
            .OrderByDescending(workspace => workspace.CreatedDate)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new WorkspaceResponse(Id: x.Id,
                Name: x.Name.Value,
                CreatedDate: x.CreatedDate,
                Role: x.WorkspaceMembers.First(member => member.UserId == request.UserId).Role.ToDto()))
            .ToListAsync(cancellationToken);

        return new GetWorkspacePageResult(result);
    }
}
