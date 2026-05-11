using Mediator;

namespace NotificationService.Application.Abstractions;

public interface ICommand : IRequest<Unit>;

public interface ICommand<out TResponse> : IRequest<TResponse>;
