using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Users.Read.Query;
using TaskService.Application.Features.Users.Read.Result;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Read.Handler;

public class GetUserByIdHandler(TaskServiceDbContext context, HybridCache cache)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery command, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"user_{command.id}";
        return await cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var user = await context.Users.FindAsync([command.id], cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException(nameof(user));
            }

            return new GetUserByIdResult(user.Id, user.Email.ToString());
        }, cancellationToken: cancellationToken);
    }
}
