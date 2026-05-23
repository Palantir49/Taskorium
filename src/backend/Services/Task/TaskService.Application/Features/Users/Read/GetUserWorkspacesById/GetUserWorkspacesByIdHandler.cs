
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Users.Read.GetUserWorkspacesById
{
    public class GetUserWorkspacesByIdHandler(TaskServiceDbContext context, HybridCache cache)
        : IRequestHandler<GetUserWorkspacesByIdQuery, GetUserWorkspacesByIdResult>
    {
        public async Task<GetUserWorkspacesByIdResult> Handle(GetUserWorkspacesByIdQuery query, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"user_workspaces_by_keycloak_id_{query.Id}";

            return await cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var existUser = await context.Users
                                    .Include(u => u.WorkspaceMembers)
                                        .ThenInclude(wm => wm.Workspace)
                                    .AsSplitQuery()
                                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken) ??
                                throw new KeyNotFoundException(
                                    $"Пользователь с таким keycloak id {query.Id} не существует");

                var userWorkspaces = existUser.WorkspaceMembers
                    .Select(x => new WorkspaceResponse(x.Workspace.Id,
                        x.Workspace.Name.Value,
                        x.Workspace.CreatedDate))
                    .ToList();

                return new GetUserWorkspacesByIdResult(userWorkspaces);
            }, cancellationToken: cancellationToken);
        }
    }
}
