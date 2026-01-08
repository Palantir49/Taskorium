using Microsoft.Extensions.DependencyInjection;

namespace TaskService.Application.Mediator
{
    public class CommandDispatcher(IServiceProvider provider) : ICommandDispatcher
    {
        public Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken ct = default)
            where TCommand : ICommand<TResult>
        {
            var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
            return handler.Handle(command, ct);
        }
    }
}
