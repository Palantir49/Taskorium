using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.WorkspaceMembers.Write.Command;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Application.Features.WorkspaceMembers.Write.UpdateWorkspaceMemberRole
{
    public class UpdateWorkspaceMemberRoleHandler(TaskServiceDbContext context, HybridCache cache, IValidator<UpdateWorkspaceMemberRoleCommand> validator)
    : IRequestHandler<UpdateWorkspaceMemberRoleCommand, AddWorkspaceMemberResult>
    {
        public async Task<AddWorkspaceMemberResult> Handle(UpdateWorkspaceMemberRoleCommand request, CancellationToken cancellationToken = default)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var user = await context.Users.Include(x => x.WorkspaceMembers)
                                          .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException($"Пользователя с id {request.UserId} не существует");

            }
            var existWorkspaceMember = user.WorkspaceMembers.FirstOrDefault(x => x.WorkspaceId == request.WorkspaceId);
            if (existWorkspaceMember is null)
            {
                throw new KeyNotFoundException($"Участника рабочей области с workspaceId {request.WorkspaceId} и userId {request.UserId} не существует");

            }
            existWorkspaceMember.SetRole(request.NewRole.ToEntity());
            await context.SaveChangesAsync();
            //инвалидируем кэш
            var cacheKey = $"user_by_keycloak_id_{user.KeycloakId}";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var userWorkspacescacheKey = $"user_workspaces_by_keycloak_id_{user.Id}";
            await cache.RemoveAsync(userWorkspacescacheKey, cancellationToken);

            var workspacemembersCacheKey = $"workspaceMembers_{request.WorkspaceId}";
            await cache.RemoveAsync(workspacemembersCacheKey, cancellationToken);

            return new AddWorkspaceMemberResult(existWorkspaceMember.WorkspaceId, existWorkspaceMember.UserId, existWorkspaceMember.Role.ToDto());
        }
    }
}
