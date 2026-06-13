using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Exceptions;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Write.AddProjectMember;

public class AddProjectMemberHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    IValidator<AddProjectMemberCommand> validator)
    : IRequestHandler<AddProjectMemberCommand, AddProjectMemberResult>
{
    public async Task<AddProjectMemberResult> Handle(AddProjectMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var existProject = await context.Projects.Include(project => project.ProjectMembers)
                               .Include(project => project.Workspace)
                               .FirstOrDefaultAsync(element => element.Id == command.ProjectId, cancellationToken) ??
                           throw new KeyNotFoundException($"Проект с id {command.UserId} не существует");
        var existUser = await context.Users.FindAsync([command.UserId], cancellationToken) ??
                        throw new KeyNotFoundException($"Пользователь с  id {command.UserId} не существует");
        var projectWorkspace =
            await context.WorkspaceMembers.FindAsync([command.UserId, existProject.WorkspaceId], cancellationToken);
        if (projectWorkspace is null)
        {
            throw new KeyNotFoundException(
                $"Пользователь с  id {command.UserId} не состоит в рабочей области с id {existProject.Workspace}");
        }

        if (existProject.ProjectMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в проекте");
        }

        var projectMember =
            ProjectMember.Create(command.ProjectId, command.UserId, command.RoleDto.ToEntity(), DateTimeOffset.UtcNow);

        await context.ProjectMembers.AddAsync(projectMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var membersCacheKey = $"projectMembers_{existProject.Id}";
        await cache.RemoveAsync(membersCacheKey, cancellationToken);

        var cacheKey = $"user_by_keycloak_id_{existUser.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        var userWorkspacescacheKey = $"user_workspace_projects_by_id_{existUser.Id}";
        await cache.RemoveAsync(userWorkspacescacheKey, cancellationToken);


        return new AddProjectMemberResult(existProject.Id,
            existUser.Id,
            projectMember.Role.ToDto());
    }
}
