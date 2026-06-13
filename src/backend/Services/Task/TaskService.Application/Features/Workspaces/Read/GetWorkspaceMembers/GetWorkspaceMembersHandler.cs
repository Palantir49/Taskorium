using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Workspace.Response;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.GetWorkspaceMembers
{
    public class GetWorkspaceMembersHandler(TaskServiceDbContext context, HybridCache cache)
        : IRequestHandler<GetWorkspaceMembersQuery, WorkspaceMembersResponse>
    {
        public async Task<WorkspaceMembersResponse> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"workspaceMembers_{request.Id}";

            return await cache.GetOrCreateAsync(cacheKey,
                async ct => await GetWorkspaceMembersFromDbAsync(request.Id, ct),
                cancellationToken: cancellationToken);
        }

        public async Task<WorkspaceMembersResponse> GetWorkspaceMembersFromDbAsync(Guid id, CancellationToken cancellationToken)
        {
            var existProject = await context.Workspaces.Include(x => x.WorkspaceMembers)
                                                         .ThenInclude(x => x.User)
                                                         .AsSplitQuery()
                                                         .AsNoTracking()
                                                         .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (existProject is null)
                throw new ArgumentNullException($"Рабочая область с id: {id} не найден");

            var members = new List<WorkspaceUserDto>();

            foreach (var x in existProject.WorkspaceMembers)
            {
                members.Add(new WorkspaceUserDto(Id: x.User.Id, KeycloakId: x.User.KeycloakId, Role: x.Role.ToDto(), JoinedAt: x.JoinedAt, Email: x.User.Email.Value, UserName: x.User.Username.Value));
            }

            return new WorkspaceMembersResponse(WorkspaceId: existProject.Id, WorkspaceName: existProject.Name.Value, Members: members);
        }
    }
}
