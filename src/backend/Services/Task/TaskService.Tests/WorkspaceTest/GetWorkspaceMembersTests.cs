using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using TaskService.Application.Features.Workspaces.Read.GetWorkspaceMembers;
using TaskService.Application.Mapping;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest;

public class GetWorkspaceMembersTests : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _context;
    private readonly GetWorkspaceMembersHandler _handler;

    public GetWorkspaceMembersTests()
    {
        _fixture = new Fixture();

        // Настраиваем AutoFixture так, чтобы он не пытался создавать реальные подключения к БД
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

        _context = new TaskServiceDbContext(options);
        var fakeCache = new FakeHybridCache();

        _fixture.Register(() => Workspace.Create(_fixture.Create<string>()));

        _fixture.Register(() => User.Create(
            keycloakId: _fixture.Create<Guid>(),
            userName: _fixture.Create<string>(),
            email: _fixture.Create<string>(),
            fullName: _fixture.Create<string>()));

        // Инициализируем хэндлер с замоканной зависимостью
        _handler = new GetWorkspaceMembersHandler(_context, fakeCache);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetWorkspaceMembersFromDbAsync_WhenWorkspaceMembersExists_ReturnsCorrectResult()
    {
        // ARRANGE
        var targetWorkspace = _fixture.Create<Workspace>();
        var otherWorkspace = _fixture.Create<Workspace>();

        var user1 = _fixture.Create<User>();
        var user2 = _fixture.Create<User>();

        var member1 = WorkspaceMember.Create(
        workspaceId: targetWorkspace.Id,
        userId: user1.Id,
        role: Domain.Entities.Enums.WorkspaceRoles.Admin);

        var member2 = WorkspaceMember.Create(
            workspaceId: targetWorkspace.Id,
            userId: user2.Id,
            role: Domain.Entities.Enums.WorkspaceRoles.Viewer);

        var noiseMember = WorkspaceMember.Create(
            workspaceId: otherWorkspace.Id,
            userId: user2.Id,
            role: Domain.Entities.Enums.WorkspaceRoles.Admin);

        _context.Users.AddRange(user1, user2);
        _context.Workspaces.AddRange(targetWorkspace, otherWorkspace);
        _context.WorkspaceMembers.AddRange(member1, member2, noiseMember);
        await _context.SaveChangesAsync(CancellationToken.None);

        //ACT
        var result = await _handler.GetWorkspaceMembersFromDbAsync(targetWorkspace.Id, CancellationToken.None);

        // ASSERT
        result.WorkspaceId.Should().Be(targetWorkspace.Id);
        result.WorkspaceName.Should().Be(targetWorkspace.Name.ToString());
        result.Members.Should().HaveCount(2);

        var resultUser1 = result.Members.First(x => x.Id == member1.UserId);
        resultUser1.Id.Should().Be(user1.Id);
        resultUser1.Role.Should().Be(member1.Role.ToDto());

        var resultUser2 = result.Members.First(x => x.Id == member2.UserId);
        resultUser2.Id.Should().Be(user2.Id);
        resultUser2.Role.Should().Be(member2.Role.ToDto());
    }

    [Fact]
    public async Task GetWorkspaceMembersFromDbAsync_WhenWorkspaceDoesNotExist_ThrowsArgumentNullException()
    {
        // ARRANGE
        var nonExistingId = _fixture.Create<Guid>();

        // ACT
        Func<Task> act = async () => await _handler.GetWorkspaceMembersFromDbAsync(nonExistingId, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Рабочая область с id: {nonExistingId} не найден");
    }
}
