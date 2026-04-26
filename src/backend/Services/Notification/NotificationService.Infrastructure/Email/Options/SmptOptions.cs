namespace NotificationService.Infrastructure.Email.Options;

public sealed record SmtpOptions
{
    public static string SectionName => "Smtp";
    public required string Host { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string FromEmail { get; init; }
    public required string FromName { get; init; }
    public bool AcceptAnyCertificate { get; init; }
    public bool UseStartTls { get; init; } = true;
}
