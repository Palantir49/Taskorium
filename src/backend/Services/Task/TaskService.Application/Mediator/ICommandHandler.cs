using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Mediator;

public interface ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}

