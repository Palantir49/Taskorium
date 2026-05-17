using Mediator;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.Notification.CreateNotification.Command;
using NotificationService.Application.Features.Notification.CreateNotification.Result;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Application.Features.Notification.CreateNotification.Handler;

public class CreateNotificationCommandHandler(
    INotificationServiceRepositoryWrapper notificationServiceRepositoryWrapper,
    ILogger<CreateNotificationCommandHandler> logger)
    : IRequestHandler<CreateNotificationCommand, CreateNotificationResult>
{
    public async ValueTask<CreateNotificationResult> Handle(CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        //check if event is exist

        var existingEvent =
            await notificationServiceRepositoryWrapper.NotificationRepository.GetByIdempotencyKeyAsync(
                request.IdempotencyKey);

        if (existingEvent is not null)
        {
            logger.LogInformation("Сообщение с данным ключом {IdempotencyKey} уже существует",
                existingEvent.EventIdempotencyKey.Value);
            return new CreateNotificationResult(existingEvent.Id);
        }

        var notificationContent = NotificationContent.Create(request.Content.Subject, request.Content.Body,
            request.Content.ActionUrl, request.Content.Metadata);

        var notificationRecipients = request.Recipients.Select(Recipient.FromIntegrationEvent).ToList();

        var notification = Domain.Aggregates.Notification.Notification.Create(
            IdempotencyKey.FromEventId(request.IdempotencyKey),
            notificationRecipients, notificationContent);

        var outBoxMessage = OutBoxMessage.CreateForNotification(notification.Id);


        //need transaction
        await notificationServiceRepositoryWrapper.BeginTransactionAsync(cancellationToken);

        try
        {
            await notificationServiceRepositoryWrapper.NotificationRepository.AddAsync(notification);
            await notificationServiceRepositoryWrapper.OutBoxRepository.AddAsync(outBoxMessage);
            await notificationServiceRepositoryWrapper.CommitAsync(cancellationToken);
        }

        catch (Exception exception)
        {
            logger.LogError(exception,
                "Ключ идемпотентности {IdempotencyKey}. Во время выполнения транзакции произошла ошибка",
                notification.EventIdempotencyKey.Value);
            await notificationServiceRepositoryWrapper.RollbackAsync(cancellationToken);
            throw;
        }


        return new CreateNotificationResult(notification.Id);
    }
}
