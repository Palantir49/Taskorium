using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.WorkspaceMembers.AddUser;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.WorkspaceMembers.AddWorkspaceMember;

public class AddWorkspaceMemberHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<AddWorkspaceMemberCommand, AddWorkspaceMemberResult>
{
    public async Task<AddWorkspaceMemberResult> Handle(AddWorkspaceMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        var existWorkspace = await context.Workspaces.Include(workspace => workspace.WorkspaceMembers)
            .FirstOrDefaultAsync(workspace => workspace.Id == command.workspaceId, cancellationToken);
        if (existWorkspace is null)
        {
            throw new KeyNotFoundException($"Рабочей области с таким id {command.workspaceId} не существует");
        }

        var existUser = await context.Users.FindAsync([command.userId], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с таким id {command.userId} не существует");
        }

        if (existWorkspace.WorkspaceMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в рабочей области");
        }

        var workspaceMember =
            WorkspaceMember.Create(command.workspaceId, command.userId, command.role, DateTimeOffset.UtcNow);

        await context.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"user_by_keycloak_id_{existUser.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);


        return new AddWorkspaceMemberResult(existWorkspace.Id,
            existUser.Id,
            new RoleDto(workspaceMember.Role.ToString()));
    }
}
