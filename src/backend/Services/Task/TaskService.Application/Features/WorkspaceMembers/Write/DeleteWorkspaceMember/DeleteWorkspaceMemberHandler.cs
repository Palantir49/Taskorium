using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.WorkspaceMembers.Write.DeleteWorkspaceMember
{
    public class DeleteWorkspaceMemberHandler(TaskServiceDbContext context, HybridCache cache) : IRequestHandler<DeleteWorkspaceMemberCommand, int>
    {
        public async Task<int> Handle(DeleteWorkspaceMemberCommand request, CancellationToken cancellationToken = default)
        {
            var member = await context.WorkspaceMembers
                .Include(m => m.User)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.WorkspaceId == request.WorkspaceId, cancellationToken)
                ?? throw new KeyNotFoundException($"Участник рабочей области не найден");

            if (member.Role == WorkspaceRoles.Creator)
                throw new InvalidOperationException($"Нельзя удалить создателя рабочей облести");

            context.Remove(member);
            int deleteCount = await context.SaveChangesAsync();

            var cacheKey = $"user_by_keycloak_id_{member.User.KeycloakId}";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var userWorkspacescacheKey = $"user_workspaces_by_keycloak_id_{member.User.Id}";
            await cache.RemoveAsync(userWorkspacescacheKey, cancellationToken);

            var workspacemembersCacheKey = $"workspaceMembers_{request.WorkspaceId}";
            await cache.RemoveAsync(workspacemembersCacheKey, cancellationToken);

            return deleteCount;
        }
    }
}
