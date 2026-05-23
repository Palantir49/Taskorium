using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Write.DeleteUserById;

public class DeleteUserByIdAsync(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<DeleteUserByIdCommand, DeleteUserByIdResult>
{
    public async Task<DeleteUserByIdResult> Handle(DeleteUserByIdCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([request.id], cancellationToken);
        if (user == null)
        {
            throw new NullReferenceException($"Пользователь с id = {request.id} не найден");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        //инвалидируем кэш
        var cacheKey = $"user_by_keycloak_id_{user.KeycloakId}";
        await cache.RemoveAsync(cacheKey, cancellationToken);
        return new DeleteUserByIdResult(user.Id,
            user.KeycloakId,
            user.Email.Value,
            user.Username.Value);
    }
}
