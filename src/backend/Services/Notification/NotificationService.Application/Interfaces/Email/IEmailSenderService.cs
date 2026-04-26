namespace NotificationService.Application.Interfaces.Email;

public interface IEmailSenderService
{
    Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
}
