using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Cache.Interfaces;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.WorkspaceMembers;
using TaskService.Application.Features.WorkspaceMembers.AddUser;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Workspaces.Create;

public class AddProjectMemberHandler(TaskServiceDbContext context, IUserCache cache)
    : IRequestHandler<AddProjectMemberCommand, AddProjectMemberResult>
{
    public async Task<AddProjectMemberResult> Handle(AddProjectMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        var existProject = await context.Projects.Include(project => project.ProjectMembers)
            .FirstOrDefaultAsync(element => element.Id == command.ProjectId, cancellationToken);
        if (existProject is null)
        {
            throw new KeyNotFoundException($"Проект с id {command.UserId} не существует");
        }

        var existUser = await context.Users.FindAsync([command.UserId], cancellationToken);
        if (existUser is null)
        {
            throw new KeyNotFoundException($"Пользователь с  id {command.UserId} не существует");
        }

        if (existProject.ProjectMembers.Any(x => x.UserId == existUser.Id))
        {
            throw new ConflictException("Пользователь уже состоит в рабочей области");
        }

        var projectMember =
            ProjectMember.Create(command.ProjectId, command.UserId, command.RoleDto.ToEntity(), DateTimeOffset.UtcNow);

        await context.ProjectMembers.AddAsync(projectMember, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        await cache.InvalidateUserByKeycloakIdCacheAsync(existUser.KeycloakId);
        return new AddProjectMemberResult(existProject.Id,
            existUser.Id,
            projectMember.Role.ToDto());
    }
}
