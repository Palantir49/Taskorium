using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Mediator;

public interface ICommandDispatcher
{
    Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>;
}
