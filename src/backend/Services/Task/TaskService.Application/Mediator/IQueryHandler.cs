using System;
using System.Collections.Generic;
using System.Text;


namespace TaskService.Application.Mediator;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}

