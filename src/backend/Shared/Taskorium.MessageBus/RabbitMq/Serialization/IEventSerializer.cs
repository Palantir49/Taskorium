using Taskorium.MessageBus.Abstractions;

namespace Taskorium.MessageBus.RabbitMq.Serialization;

public interface IEventSerializer
{
    byte[] Serialize<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
    TEvent Deserialize<TEvent>(ReadOnlyMemory<byte> body) where TEvent : IntegrationEvent;
}
