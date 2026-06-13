using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Exceptions;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;

public class AddWorkspaceMemberHandler(TaskServiceDbContext context, HybridCache cache, IValidator<AddWorkspaceMemberCommand> validator)
    : IRequestHandler<AddWorkspaceMemberCommand, AddWorkspaceMemberResult>
{
    public async Task<AddWorkspaceMemberResult> Handle(AddWorkspaceMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var existWorkspace = await context.Workspaces.Include(workspace => workspace.WorkspaceMembers)
            .FirstOrDefaultAsync(workspace => workspace.Id == command.WorkspaceId, cancellationToken);
        if (existWorkspace is null)
        {
            throw new KeyNotFoundException($"Рабочей области с таким id {command.WorkspaceId} не существует");
        }

        var existUser = await context.Users.FindAsync([command.UserId], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с таким id {command.UserId} не существует");
        }

        if (existWorkspace.WorkspaceMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в рабочей области");
        }

        var workspaceMember =
            WorkspaceMember.Create(command.WorkspaceId, command.UserId, command.Role.ToEntity());

        await context.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var workspaceMemberscacheKey = $"workspaceMembers_{existWorkspace.Id}";
        await cache.RemoveAsync(workspaceMemberscacheKey, cancellationToken);

        var cacheKey = $"user_by_keycloak_id_{existUser.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        var userWorkspacesCacheKey = $"user_workspaces_by_keycloak_id_{existUser.Id}";
        await cache.RemoveAsync(userWorkspacesCacheKey, cancellationToken);

        return new AddWorkspaceMemberResult(existWorkspace.Id,
            existUser.Id,
            workspaceMember.Role.ToDto());
    }
}
