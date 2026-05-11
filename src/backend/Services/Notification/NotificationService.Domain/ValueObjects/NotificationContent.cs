namespace NotificationService.Domain.ValueObjects;

public record NotificationContent
{
    private NotificationContent(
        string subject,
        string body,
        string? actionUrl,
        Dictionary<string, string>? metadata)
    {
        Subject = subject;
        Body = body;
        ActionUrl = actionUrl;
        Metadata = metadata ?? [];
    }

    public string Subject { get; private set; }
    public string Body { get; private set; }

    public string? ActionUrl { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }

    public static NotificationContent Create(
        string subject,
        string body,
        string? actionUrl = null,
        Dictionary<string, string>? metadata = null)
    {
        return new NotificationContent(
            subject, body, actionUrl, metadata);
    }
}
