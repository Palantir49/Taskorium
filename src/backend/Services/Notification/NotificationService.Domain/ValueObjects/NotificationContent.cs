namespace NotificationService.Domain.ValueObjects;

public record NotificationContent
{
    private NotificationContent(
        string subject,
        string body,
        string? actionUrl,
        Dictionary<string, object>? metadata)
    {
        Subject = subject;
        Body = body;
        ActionUrl = actionUrl;
        Metadata = metadata ?? [];
    }

    public string Subject { get; }
    public string Body { get; }

    public string? ActionUrl { get; }
    public Dictionary<string, object> Metadata { get; }

    public static NotificationContent Create(
        string subject,
        string body,
        string? actionUrl = null,
        Dictionary<string, object>? metadata = null)
    {
        return new NotificationContent(
            subject, body, actionUrl, metadata);
    }
}
