using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Moq;
using TaskService.Application.Features.Workspaces.Read.GetWorkspaceById;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest;

public class WorkspaceGetByIdTests : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _context;
    private readonly GetWorkspaceByIdHandler _handler;

    public WorkspaceGetByIdTests()
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
        // Инициализируем хэндлер с замоканной зависимостью
        _handler = new GetWorkspaceByIdHandler(_context, fakeCache);
    }

    [Fact]
    public async Task GetWorkspaceFromDbAsync_WhenWorkspaceExists_ReturnsCorrectResult()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();

        // Сохраняем в In-Memory БД (эмулируем существующую запись)
        _context.Workspaces.Add(workspace);
        await _context.SaveChangesAsync(CancellationToken.None);

        // ACT
        // Вызываем вынесенный метод напрямую, минуя кэш
        var result = await _handler.GetWorkspaceFromDbAsync(workspace.Id, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(workspace.Id);
        result.Name.Should().Be(workspace.Name.ToString());
    }

    [Fact]
    public async Task GetWorkspaceFromDbAsync_WhenWorkspaceDoesNotExist_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var nonExistingId = _fixture.Create<Guid>();
        // В БД ничего нет

        // ACT
        Func<Task> act = async () => await _handler.GetWorkspaceFromDbAsync(nonExistingId, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Рабочая область с id: {nonExistingId} не найдена");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
