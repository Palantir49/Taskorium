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

    public ChannelType Type { get; private set; }
    public string Address { get; private set; }
    public ChannelStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? LastAttemptAt { get; private set; }
    public int RetryCount { get; private init; }
    public int MaxRetries { get; private set; }

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
