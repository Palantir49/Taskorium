
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Users.Read.GetUserProjectsById
{
    public class GetUserProjectsByIdHandler(TaskServiceDbContext context, HybridCache cache)
        : IRequestHandler<GetUserProjectsByIdQuery, GetUserProjectsByIdResult>
    {
        public async Task<GetUserProjectsByIdResult> Handle(GetUserProjectsByIdQuery query, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"user_workspace_projects_by_id_{query.Id}";

            return await cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var existUser = await context.Users
                                    .Include(u => u.ProjectMembers)
                                        .ThenInclude(p => p.Project)
                                    .AsSplitQuery()
                                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken) ??
                                throw new KeyNotFoundException(
                                    $"Пользователь с таким keycloak id {query.Id} не существует");

                var userWorkspaces = existUser.ProjectMembers
                    .Select(x => new UserProjectsResponse(Id: x.ProjectId,
                                                          Role: x.Role.ToDto(),
                                                          Name: x.Project.Name.Value,
                                                          CreatedDate: x.Project.CreatedDate))
                    .ToList();

                return new GetUserProjectsByIdResult(userWorkspaces);
            }, cancellationToken: cancellationToken);
        }
    }
}
