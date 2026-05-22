using NotificationService.Domain.Aggregates.Abstracts;

namespace NotificationService.Domain.Aggregates.Outbox;

// Domain/Aggregates/Outbox/OutBoxMessage.cs
public class OutBoxMessage : AggregateRoot
{
    private OutBoxMessage() { }

    public Guid NotificationId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public DateTimeOffset? LockedAt { get; private set; } // ← когда взяли в работу
    public string? LockedBy { get; private set; } // ← кто взял (instanceId)
    public bool Processed { get; private set; }
    public int RetryCount { get; private set; }
    public string? LastError { get; private set; }

    public static OutBoxMessage CreateForNotification(Guid notificationId)
    {
        return new OutBoxMessage
        {
            Id = Guid.CreateVersion7(),
            NotificationId = notificationId,
            CreatedAt = DateTimeOffset.UtcNow,
            Processed = false,
            RetryCount = 0
        };
    }

    public void MarkProcessed()
    {
        Processed = true;
        ProcessedAt = DateTimeOffset.UtcNow;
        LockedAt = null;
        LockedBy = null;
    }

    public void MarkFailed(string error)
    {
        RetryCount++;
        LastError = error;
        LockedAt = null; // освобождаем блокировку при ошибке
        LockedBy = null;
    }
}
