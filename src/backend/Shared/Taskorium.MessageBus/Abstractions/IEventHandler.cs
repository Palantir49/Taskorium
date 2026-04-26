namespace Taskorium.MessageBus.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : IntegrationEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}
