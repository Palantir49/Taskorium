using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.Interfaces.NotificationSenders;
using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Enums;
using NotificationService.Domain.ValueObjects;
using NotificationService.Infrastructure.NotificationSenders.Options;
using Taskorium.ServiceDefaults.Result;

namespace NotificationService.Infrastructure.NotificationSenders.Services;

public sealed class EmailSenderService(
    IOptions<SmtpOptions> options,
    ILogger<EmailSenderService> logger
) : IChannelSender
{
    public ChannelType ChannelType => ChannelType.Email;

    public async Task<Result<bool>> SendAsync(
        NotificationChannel channel,
        RecipientNotification rn,
        CancellationToken ct = default)
    {
        try
        {
            var secureSocketOptions = options.Value.UseStartTls
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.SslOnConnect;

            using var client = BuildSmtpClient();
            await client.ConnectAsync(options.Value.Host, options.Value.Port, secureSocketOptions, ct);
            await client.AuthenticateAsync(options.Value.Username, options.Value.Password, ct);
            using var message = BuildMailMessage(channel.Address, rn);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Ошибка отправки сообщения по адресу {Address}", channel.Address);

            return Result<bool>.Failure(ex.Message);
        }
    }

    private static SmtpClient BuildSmtpClient()
    {
        return new SmtpClient { ServerCertificateValidationCallback = delegate { return true; } };
    }

    private MimeMessage BuildMailMessage(string to, RecipientNotification rn)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(options.Value.FromName, options.Value.FromEmail));
        message.To.Add(new MailboxAddress(rn.Recipient.FullName, to));
        message.Subject = rn.Content.Subject;
        message.Body = new TextPart("plain") { Text = GetTextContent(rn) };


        return message;
    }


    private static string GetTextContent(RecipientNotification rn)
    {
        return !string.IsNullOrWhiteSpace(rn.Content.ActionUrl)
            ? $"{rn.Content.Body}\n\n{rn.Content.ActionUrl}"
            : rn.Content.Body;
    }
}
