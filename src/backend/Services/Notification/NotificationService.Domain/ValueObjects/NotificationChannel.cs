using NotificationService.Domain.Enums;

namespace NotificationService.Domain.ValueObjects;

public record NotificationChannel
{
    private NotificationChannel(
        ChannelType type,
        string address,
        ChannelStatus status = ChannelStatus.Pending,
        string? errorMessage = null,
        DateTime? sentAt = null,
        DateTime? lastAttemptAt = null,
        int retryCount = 0,
        int maxRetries = 3)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Channel address cannot be empty");
        }

        Type = type;
        Address = address;
        Status = status;
        ErrorMessage = errorMessage;
        SentAt = sentAt;
        LastAttemptAt = lastAttemptAt;
        RetryCount = retryCount;
        MaxRetries = maxRetries;
    }

    public ChannelType Type { get; }
    public string Address { get; }
    public ChannelStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTime? SentAt { get; init; }
    public DateTime? LastAttemptAt { get; init; }
    public int RetryCount { get; init; }
    public int MaxRetries { get; }

    public bool CanRetry => RetryCount < MaxRetries &&
                            Status == ChannelStatus.Failed;

    public static NotificationChannel Create(ChannelType type, string address)
    {
        return new NotificationChannel(type, address);
    }

    public NotificationChannel MarkAsSent(DateTime sentAt)
    {
        return this with { Status = ChannelStatus.Sent, SentAt = sentAt, LastAttemptAt = sentAt };
    }

    public NotificationChannel MarkAsFailed(string error)
    {
        return this with { Status = ChannelStatus.Failed, ErrorMessage = error, LastAttemptAt = DateTime.UtcNow };
    }

    public NotificationChannel MarkAsSkipped(string reason)
    {
        return this with { Status = ChannelStatus.Skipped, ErrorMessage = reason };
    }

    public NotificationChannel RecordRetryAttempt()
    {
        return this with
        {
            RetryCount = RetryCount + 1,
            LastAttemptAt = DateTime.UtcNow,
            Status = ChannelStatus.Pending
        };
    }
}
