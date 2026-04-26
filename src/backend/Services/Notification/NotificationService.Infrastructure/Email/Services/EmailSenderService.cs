using NotificationService.Application.Interfaces.Email;

namespace NotificationService.Infrastructure.Email.Services;

public sealed class EmailSenderService : IEmailSenderService
{
    //TODO via MailKit (maybe use dto, factory method, etc) 
    public async Task SendEmailAsync(string email, string subject, string message,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
