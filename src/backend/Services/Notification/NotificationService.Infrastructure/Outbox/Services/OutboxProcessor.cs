using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.NotificationSenders;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Infrastructure.Outbox.Options;

namespace NotificationService.Infrastructure.Outbox.Services;

public sealed class OutboxProcessor(
    ILogger<OutboxProcessor> logger,
    IOptions<OutboxOptions> outBoxOptions,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly string _instanceId = $"{Environment.MachineName}-{Guid.NewGuid()}";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var loggingScope =
            logger.BeginScope(new Dictionary<string, string?> { { "OutboxInstanceId", _instanceId } });

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation($"Начало работы {nameof(OutboxProcessor)}");
                await ProcessBatchAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "В процессе outbox обработки сообщений произошла ошибка");
                continue;
            }

            await Task.Delay(outBoxOptions.Value.PollingInterval, stoppingToken);
        }
    }


    private async Task ProcessBatchAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();

        var notificationRepositoryWrapper =
            scope.ServiceProvider.GetRequiredService<INotificationServiceRepositoryWrapper>();
        var notificationSenderService = scope.ServiceProvider.GetRequiredService<INotificationSender>();

        //get all free messages
        var acquiredMessages =
            await notificationRepositoryWrapper.OutBoxRepository.AcquireBatchAsync(outBoxOptions.Value.BatchSize,
                outBoxOptions.Value.MaxRetries, _instanceId, stoppingToken);

        logger.LogInformation("Получено {CountMessages} сообщений для обработки", acquiredMessages.Count);

        if (acquiredMessages.Count == 0)
        {
            return;
        }

        foreach (var message in acquiredMessages)
        {
            try
            {
                await ProcessOneAsync(message, notificationRepositoryWrapper, notificationSenderService, stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "В процессе обработки сообщения произошла ошибка");
                throw;
            }
        }
    }


    private async Task ProcessOneAsync(OutBoxMessage message,
        INotificationServiceRepositoryWrapper notificationServiceRepositoryWrapper,
        INotificationSender notificationSenderService, CancellationToken stoppingToken)
    {
        using var loggerScope = logger.BeginScope(new Dictionary<string, object>
            {
                ["OutBoxMessageId"] = message.Id, ["NotificationId"] = message.NotificationId
            }
        );

        logger.LogInformation("Начало обработки сообщения");

        var notification =
            await notificationServiceRepositoryWrapper.NotificationRepository.GetByIdAsync(message.NotificationId);

        if (notification == null)
        {
            logger.LogError("В результате обработки сообщения произошла ошибка. Получено пусто сообщение");
            throw new ArgumentNullException(nameof(notification));
        }

        logger.LogInformation("Получено сообщение для дальнейшей отправки по каналам получателя");
        logger.LogInformation("Начало отправки сообщения по каналам получателей");
        await notificationSenderService.SendAsync(notification, stoppingToken);
        logger.LogInformation("Конец отправки сообщения по каналам получателей");


        try
        {
            logger.LogInformation("Начало обновления информации в БД об отправленных сообщениях");
            await notificationServiceRepositoryWrapper.BeginTransactionAsync(stoppingToken);
            message.MarkProcessed();
            notification.MarkProcessed();
            await notificationServiceRepositoryWrapper.OutBoxRepository.UpdateOneAsync(message);
            await notificationServiceRepositoryWrapper.NotificationRepository.UpdateOneAsync(notification);
            await notificationServiceRepositoryWrapper.CommitAsync(stoppingToken);
            logger.LogInformation("Конец обновления информации в БД об отправленном сообщении");
        }

        catch (Exception exception)
        {
            logger.LogError(exception,
                "Во время обновления информации в БД об отправленных сообщениях произошла ошибка");
            await notificationServiceRepositoryWrapper.RollbackAsync(stoppingToken);
            throw;
        }
    }
}
