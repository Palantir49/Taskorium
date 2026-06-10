using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Interfaces;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

public class CreateWorkspaceHandler(TaskServiceDbContext context, HybridCache cache, ICurrentUserContext userContext)
    : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {
        var workspace = Workspace.Create(
            command.Name
        );
        await context.Workspaces.AddAsync(workspace, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var existUser = await context.Users.FindAsync([userContext.User.Id], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с таким id {userContext.User.Id} не существует");
        }

        var workspaceMember =
            WorkspaceMember.Create(workspace.Id, userContext.User.Id, WorkspaceRolesDto.Creator.ToEntity());

        await context.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"user_by_keycloak_id_{existUser.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        return new CreateWorkspaceResult(
            workspace.Id,
            workspace.Name.ToString(),
            WorkspaceRolesDto.Creator);
    }
}
