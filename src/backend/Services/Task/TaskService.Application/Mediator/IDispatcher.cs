using System;

namespace TaskService.Application.Mediator;

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
}
