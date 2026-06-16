using FluentAssertions;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.Notification.CreateNotification.Command;
using NotificationService.Application.Features.Notification.CreateNotification.Handler;
using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Repositories.Interfaces.Notifications;
using NotificationService.Domain.Repositories.Interfaces.OutBox;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Taskorium.IntegrationEvents.Dto;

namespace NotificationService.Tests;

public class CreateNotificationCommandHandlerTests
{
    private readonly INotificationServiceRepositoryWrapper _repositoryWrapper;
    private readonly INotificationRepository _notificationRepository;
    private readonly IOutBoxRepository _outBoxRepository;
    private readonly CreateNotificationCommandHandler _sut;

    public CreateNotificationCommandHandlerTests()
    {
        _repositoryWrapper = Substitute.For<INotificationServiceRepositoryWrapper>();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _outBoxRepository = Substitute.For<IOutBoxRepository>();
        var logger = Substitute.For<ILogger<CreateNotificationCommandHandler>>();

        _repositoryWrapper.NotificationRepository.Returns(_notificationRepository);
        _repositoryWrapper.OutBoxRepository.Returns(_outBoxRepository);

        _sut = new CreateNotificationCommandHandler(_repositoryWrapper, logger);
    }

    private static CreateNotificationCommand CreateValidCommand(Guid? idempotencyKey = null)
    {
        return new CreateNotificationCommand(
            idempotencyKey ?? Guid.CreateVersion7(),
            DateTimeOffset.UtcNow,
            new NotificationEventContent
            {
                Subject = "Subject",
                Body = "Body text",
                ActionUrl = "https://example.com/action",
                Metadata = new Dictionary<string, string> { ["key"] = "value" }
            },
            [
                new NotificationRecipient
                {
                    UserId = "user-1",
                    FullName = "John Doe",
                    Email = "john@example.com",
                    Phone = "+123456789",
                    IsMuted = false,
                    PreferredChannels = ["Email"]
                }
            ]);
    }

    [Fact]
    public async Task Handle_WhenEventWithIdempotencyKeyAlreadyExists_ReturnsExistingId_AndDoesNotStartTransaction()
    {
        var command = CreateValidCommand();

        var existingNotification = Notification.Create(
            IdempotencyKey.FromEventId(command.IdempotencyKey),
            [Recipient.FromIntegrationEvent(command.Recipients[0])],
            NotificationContent.Create("Subj", "Body"));

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns(existingNotification);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.NotificationId.Should().Be(existingNotification.Id);

        await _repositoryWrapper.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _notificationRepository.DidNotReceive().AddAsync(Arg.Any<Notification>());
        await _outBoxRepository.DidNotReceive().AddAsync(Arg.Any<OutBoxMessage>());
        await _repositoryWrapper.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_CreatesNotificationAndOutboxMessage_AndCommits()
    {
        var command = CreateValidCommand();

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.NotificationId.Should().NotBe(Guid.Empty);

        await _repositoryWrapper.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());

        await _notificationRepository.Received(1)
            .AddAsync(Arg.Is<Notification>(n =>
                n.Id == result.NotificationId &&
                n.EventIdempotencyKey.Value == command.IdempotencyKey));

        await _outBoxRepository.Received(1)
            .AddAsync(Arg.Is<OutBoxMessage>(m => m.NotificationId == result.NotificationId));

        await _repositoryWrapper.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _repositoryWrapper.DidNotReceive().RollbackAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAddNotificationThrows_RollsBackTransaction_AndRethrows()
    {
        var command = CreateValidCommand();

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        var thrown = new InvalidOperationException("db error");

        _notificationRepository
            .AddAsync(Arg.Any<Notification>())
            .ThrowsAsync(thrown);

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("db error");

        await _repositoryWrapper.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _repositoryWrapper.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _repositoryWrapper.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAddOutboxThrows_RollsBackTransaction_AndRethrows()
    {
        var command = CreateValidCommand();

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        var thrown = new InvalidOperationException("outbox error");

        _outBoxRepository
            .AddAsync(Arg.Any<OutBoxMessage>())
            .ThrowsAsync(thrown);

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("outbox error");

        await _repositoryWrapper.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _repositoryWrapper.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitThrows_RollsBackTransaction_AndRethrows()
    {
        var command = CreateValidCommand();

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        var thrown = new InvalidOperationException("commit error");

        _repositoryWrapper
            .CommitAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(thrown);

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("commit error");

        await _repositoryWrapper.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CallsGetByIdempotencyKeyWithCorrectKey()
    {
        var command = CreateValidCommand(Guid.CreateVersion7());

        _notificationRepository
            .GetByIdempotencyKeyAsync(Arg.Any<Guid>())
            .Returns((Notification?)null);

        await _sut.Handle(command, CancellationToken.None);

        await _notificationRepository.Received(1)
            .GetByIdempotencyKeyAsync(command.IdempotencyKey);
    }

    [Fact]
    public async Task Handle_CreatedNotification_HasCorrectIdempotencyKeyAndRecipientCount()
    {
        var command = CreateValidCommand();

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        Notification? capturedNotification = null;

        _notificationRepository
            .When(r => r.AddAsync(Arg.Any<Notification>()))
            .Do(call => capturedNotification = call.Arg<Notification>());

        await _sut.Handle(command, CancellationToken.None);

        capturedNotification.Should().NotBeNull();
        capturedNotification!.EventIdempotencyKey.Value.Should().Be(command.IdempotencyKey);
        capturedNotification.RecipientNotifications.Should().HaveCount(command.Recipients.Count);
        capturedNotification.Status.Should().Be(NotificationStatus.Pending);
    }

    [Fact]
    public async Task Handle_WhenRecipientsListIsEmpty_ThrowsBeforeStartingTransaction()
    {
        var command = CreateValidCommand() with { Recipients = [] };

        _notificationRepository
            .GetByIdempotencyKeyAsync(command.IdempotencyKey)
            .Returns((Notification?)null);

        var act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotificationDomainException>();

        await _repositoryWrapper.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _repositoryWrapper.DidNotReceive().RollbackAsync(Arg.Any<CancellationToken>());
    }
}
