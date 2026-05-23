using System.Text.Json;
using TaskService.Application.Outbox.Interfaces;

namespace TaskService.Application.Outbox.Services;

/// <summary>
/// Каркас JSON-сериализатора для Outbox.
/// </summary>
public class SystemTextJsonOutboxSerializer : IOutboxSerializer
{
    public SystemTextJsonOutboxSerializer(JsonSerializerOptions? options = null)
    {
        JsonOptions = options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    protected JsonSerializerOptions JsonOptions { get; }

    public T Deserialize<T>(string payload)
    {
        return JsonSerializer.Deserialize<T>(payload, JsonOptions) 
               ?? throw new InvalidOperationException($"Не удалось десериализовать payload в тип {typeof(T).Name}");
    }

    public string Serialize<T>(T payload)
    {
        return JsonSerializer.Serialize(payload, JsonOptions);
    }
}
