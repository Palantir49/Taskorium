using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Projects.Write.AddProjectMember;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.ProjectMembers.Write.UpdateProjectMemberRole;

public class UpdateProjectMemberRoleHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    IValidator<UpdateProjectMemberRoleCommand> validator)
    : IRequestHandler<UpdateProjectMemberRoleCommand, AddProjectMemberResult>
{
    public async Task<AddProjectMemberResult> Handle(UpdateProjectMemberRoleCommand request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        var user = await context.Users.Include(x => x.ProjectMembers)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"Пользователя с id {request.UserId} не существует");
        }

        var existProjectMember = user.ProjectMembers.FirstOrDefault(x => x.ProjectId == request.ProjectId);
        if (existProjectMember is null)
        {
            throw new KeyNotFoundException(
                $"Участника проекта с projectId {request.ProjectId} и userId {request.UserId} не существует");
        }

        existProjectMember.SetRole(request.NewRole.ToEntity());
        await context.SaveChangesAsync();
        //инвалидируем кэш
        var membersCacheKey = $"projectMembers_{existProjectMember.ProjectId}";
        await cache.RemoveAsync(membersCacheKey, cancellationToken);

        var cacheKey = $"user_by_keycloak_id_{user.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);

        var userWorkspacescacheKey = $"user_workspace_projects_by_id_{user.Id}";
        await cache.RemoveAsync(userWorkspacescacheKey, cancellationToken);

        var userProjectsByWorkSpaceCacheKey = $"projects_by_workspace_{request.ProjectId}_{request.UserId}";
        await cache.RemoveAsync(userProjectsByWorkSpaceCacheKey, cancellationToken);

        return new AddProjectMemberResult(existProjectMember.ProjectId, existProjectMember.UserId,
            existProjectMember.Role.ToDto());
    }
}
