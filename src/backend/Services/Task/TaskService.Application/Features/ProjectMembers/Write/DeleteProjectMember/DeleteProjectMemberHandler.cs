using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.ProjectMembers.Write.DeleteProjectMember
{
    public class DeleteProjectMemberHandler(TaskServiceDbContext context, HybridCache cache) : IRequestHandler<DeleteProjectMemberCommand, int>
    {
        public async Task<int> Handle(DeleteProjectMemberCommand request, CancellationToken cancellationToken = default)
        {
            var member = await context.ProjectMembers
                .Include(m => m.User)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.ProjectId == request.ProjectId, cancellationToken)
                ?? throw new KeyNotFoundException($"Участник рабочей области не найден");

            if (member.Role == ProjectRoles.Creator)
                throw new InvalidOperationException($"Нельзя удалить создателя проекта");

            context.Remove(member);
            int deleteCount = await context.SaveChangesAsync(cancellationToken);

            var membersCacheKey = $"projectMembers_{member.ProjectId}";
            await cache.RemoveAsync(membersCacheKey, cancellationToken);

            var cacheKey = $"user_by_keycloak_id_{member.User.KeycloakId}";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var userWorkspacescacheKey = $"user_workspace_projects_by_id_{member.User.Id}";
            await cache.RemoveAsync(userWorkspacescacheKey, cancellationToken);

            return deleteCount;
        }
    }
}
