namespace Taskorium.MessageBus.Abstractions;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}
