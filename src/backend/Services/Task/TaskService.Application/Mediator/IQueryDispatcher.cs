using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;

namespace TaskService.Application.Abstractions;

public interface IQueryDispatcher
{
    Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default) where TQuery : IQuery<TResult>;

}
