using Taskorium.MessageBus.Abstractions;
using Taskorium.MessageBus.RabbitMq.Options;

namespace Taskorium.MessageBus.RabbitMq.Topology;

public interface IEventTopologyRegistry
{
    RabbitMqPublishOptions GetPublishOptions<TEvent>() where TEvent : IntegrationEvent;
}
