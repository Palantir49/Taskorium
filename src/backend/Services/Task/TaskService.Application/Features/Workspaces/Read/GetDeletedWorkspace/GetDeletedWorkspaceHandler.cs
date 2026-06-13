using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.GetDeletedWorkspace;

public class GetDeletedWorkspaceHandler(TaskServiceDbContext context) : IRequestHandler<GetDeletedWorkspacePageQuery, GetDeletedWorkspacePageResult>
{
    public async Task<GetDeletedWorkspacePageResult> Handle(GetDeletedWorkspacePageQuery query, CancellationToken cancellationToken)
    {
        var deletedWorkspaces = context.Workspaces.AsNoTracking()
                                                  .IgnoreQueryFilters(["SoftDelete"])
                                                  .Where(x => x.IsDeleted)
                                                  .OrderByDescending(p => p.DeletedAt)
                                                  .Skip(query.Skip)
                                                  .Take(query.Take);

        var response = await deletedWorkspaces.Select(x => new DeletedWorkspaceResponse(Id: x.Id,
                                                                                        Name: x.Name.Value,
                                                                                        DeletedAt: x.DeletedAt!.Value))
                                              .ToListAsync();
        return new GetDeletedWorkspacePageResult(workspaces: response);
    }
}
