namespace NotificationService.Domain.Aggregates.Abstracts;

public abstract class AggregateRoot
{
    public Guid Id { get; protected init; }
}
