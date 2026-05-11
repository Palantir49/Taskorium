namespace NotificationService.Infrastructure.Outbox.Options;

public sealed record OutboxOptions
{
    public static string SectionName => "Outbox";
    public TimeSpan PollingInterval { get; init; } = TimeSpan.FromSeconds(5);
    public int BatchSize { get; init; } = 50;

    public int MaxRetries { get; init; } = 5;
}
