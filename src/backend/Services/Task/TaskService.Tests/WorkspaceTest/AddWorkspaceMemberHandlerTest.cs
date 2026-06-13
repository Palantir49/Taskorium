using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.Workspaces.Write.AddWorkspaceMember;
using TaskService.Application.Mapping;
using TaskService.Application.Validators.WorkspaceMember;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Tests.WorkspaceTest;

public class AddWorkspaceMemberHandlerTest : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _dbContext;
    private readonly FakeHybridCache _fakeCache;
    private readonly AddWorkspaceMemberCommandValidator _validator;
    private readonly AddWorkspaceMemberHandler _handler;
    private bool _disposed;

    public AddWorkspaceMemberHandlerTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() => Workspace.Create(_fixture.Create<string>()));

        _fixture.Register(() => User.Create(
            keycloakId: _fixture.Create<Guid>(),
            userName: _fixture.Create<string>(),
            email: _fixture.Create<string>(),
            fullName: _fixture.Create<string>()));

        var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new TaskServiceDbContext(options);

        _validator = new AddWorkspaceMemberCommandValidator();

        _fakeCache = new FakeHybridCache();

        _handler = new AddWorkspaceMemberHandler(_dbContext, _fakeCache, _validator);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _dbContext?.Dispose();
        }
        _disposed = true;
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenWorkspaceNotFound_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var nonExistingWorkspaceId = _fixture.Create<Guid>();
        var user = _fixture.Create<User>();
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new AddWorkspaceMemberCommand(
            nonExistingWorkspaceId,
            user.Id,
            WorkspaceRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Рабочей области с таким id {nonExistingWorkspaceId} не существует");
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenUserNotFound_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();
        _dbContext.Workspaces.Add(workspace);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var nonExistingUserId = _fixture.Create<Guid>();
        var command = new AddWorkspaceMemberCommand(
            workspace.Id,
            nonExistingUserId,
            WorkspaceRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{nonExistingUserId}*не существует*");
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenUserAlreadyInWorkspace_ThrowsConflictException()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();
        var user = _fixture.Create<User>();

        var existingMember = WorkspaceMember.Create(
            workspace.Id,
            user.Id,
            WorkspaceRoles.Admin);

        _dbContext.Workspaces.Add(workspace);
        _dbContext.Users.Add(user);
        _dbContext.WorkspaceMembers.Add(existingMember);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new AddWorkspaceMemberCommand(
            workspace.Id,
            user.Id,
            WorkspaceRoles.Viewer.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Пользователь уже состоит в рабочей области");
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenValidCommand_AddsMemberAndReturnsResult()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();
        var user = _fixture.Create<User>();

        _dbContext.Workspaces.Add(workspace);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var role = WorkspaceRoles.Admin;
        var command = new AddWorkspaceMemberCommand(
            workspace.Id,
            user.Id,
            role.ToDto());

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.WorkspaceId.Should().Be(workspace.Id);
        result.UserId.Should().Be(user.Id);

        var memberInDb = await _dbContext.WorkspaceMembers
            .FirstOrDefaultAsync(m => m.WorkspaceId == workspace.Id && m.UserId == user.Id, CancellationToken.None);

        memberInDb.Should().NotBeNull();
        memberInDb.Role.Should().Be(role);
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenValidationWorkspaceIdFails_ThrowsValidationException()
    {
        // ARRANGE
        var command = new AddWorkspaceMemberCommand(
            Guid.Empty,
            _fixture.Create<Guid>(),
            WorkspaceRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AddWorkspaceMember_WhenValidationUserIdFails_ThrowsValidationException()
    {
        // ARRANGE
        var command = new AddWorkspaceMemberCommand(
            _fixture.Create<Guid>(), 
            Guid.Empty,
            WorkspaceRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ValidationException>();
    }
}
