
using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Abstractions;

namespace TaskService.Application.Mediator;

public class QueryDispatcher(IServiceProvider provider) : IQueryDispatcher
{
    public Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResult>
    {
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return handler.Handle(query, ct);
    }
}
