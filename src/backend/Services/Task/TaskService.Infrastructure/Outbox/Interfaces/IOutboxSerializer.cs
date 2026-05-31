namespace TaskService.Infrastructure.Outbox.Interfaces;

/// <summary>
/// Контракт сериализатора для Outbox payload.
/// </summary>
public interface IOutboxSerializer
{
    /// <summary>
    /// Сериализовать payload события в строку для хранения в БД.
    /// </summary>
    string Serialize<T>(T payload);

    /// <summary>
    /// Десериализовать строку payload обратно в объект нужного типа.
    /// </summary>
    T Deserialize<T>(string payload);
}
