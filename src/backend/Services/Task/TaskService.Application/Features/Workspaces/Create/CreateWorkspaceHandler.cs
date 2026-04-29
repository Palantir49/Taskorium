using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Cache.Interfaces;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Workspaces.Create;

public class CreateWorkspaceHandler(TaskServiceDbContext context, IUserCache cache)
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
        var existUser = await context.Users.FindAsync([command.OwnerId], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с таким id {command.OwnerId} не существует");
        }
        var workspaceMember = WorkspaceMember.Create(workspace.Id, command.OwnerId, RolesDto.Creator.ToEntity());

        await context.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        await cache.InvalidateUserByKeycloakIdCacheAsync(existUser.KeycloakId);

        return new CreateWorkspaceResult(
            workspace.Id,
            workspace.Name.ToString());
    }
}
