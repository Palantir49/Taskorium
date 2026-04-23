using System.Text.Json;
using Taskorium.MessageBus.Abstractions;

namespace Taskorium.MessageBus.RabbitMq.Serialization;

public sealed class SystemTextJsonEventSerializer : IEventSerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public byte[] Serialize<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        return JsonSerializer.SerializeToUtf8Bytes(@event, JsonOptions);
    }


    public TEvent Deserialize<TEvent>(ReadOnlyMemory<byte> body) where TEvent : IntegrationEvent
    {
        return JsonSerializer.Deserialize<TEvent>(body.Span, JsonOptions)
               ?? throw new InvalidOperationException($"Cannot deserialize {typeof(TEvent).FullName}");
    }
}
