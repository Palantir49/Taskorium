using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Workspaces.Write.UpdateWorkspaceName;
using TaskService.Application.Validators.Workspace;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest.Write;

public class UpdateWorkspaceNameHandlerTest : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _context;
    private readonly FakeHybridCache _fakeCache;
    private readonly UpdateWorkspaceNameCommandValidator _validator;
    private readonly UpdateWorkspaceNameHandler _handler;
    private bool _disposed;

    public UpdateWorkspaceNameHandlerTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() => Workspace.Create(_fixture.Create<string>()));

        var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new TaskServiceDbContext(options);

        _validator = new UpdateWorkspaceNameCommandValidator();

        _fakeCache = new FakeHybridCache();

        _handler = new UpdateWorkspaceNameHandler(_context, _fakeCache, _validator);
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
            _context?.Dispose();
        }
        _disposed = true;
    }

    public static IEnumerable<object[]> InvalidCommandData()
    {
        yield return new object[] {
                new UpdateWorkspaceNameCommand(Guid.Empty, string.Empty), "Id"
            };

        yield return new object[] {
                new UpdateWorkspaceNameCommand(Guid.NewGuid(), string.Empty), "Name"
            };

        yield return new object[] {
                new UpdateWorkspaceNameCommand(Guid.NewGuid(), "%^&*()"), "Name"
            };

        yield return new object[] {
                new UpdateWorkspaceNameCommand(Guid.NewGuid(), "йwertyuiopйwertyuiopйwertyuiopйwertyuiopйwertyuiopйwertyuiop"), "Name"
            };
    }

    [Theory]
    [MemberData(nameof(InvalidCommandData))]
    public async Task UpdateWorkspaceNameHandler_WhenValidationFails_ThrowsValidationException(UpdateWorkspaceNameCommand command, string expectedInvalidProperty)
    {
        // ARRANGE
        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().Contain(e => e.PropertyName == expectedInvalidProperty);
    }

    [Fact]
    public async Task UpdateWorkspaceNameHandler_WhenWorkspaceNotExist_ThrowKeyNotFoundException()
    {
        // ARRANGE
        var command = new UpdateWorkspaceNameCommand(Guid.NewGuid(), "Тестовый тест");
        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task UpdateWorkspaceNameHandler_WhenWorkspaceValidAndExists_UpdateWorkspaceAndReturnResult()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();
        _context.Workspaces.Add(workspace);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new UpdateWorkspaceNameCommand(workspace.Id, "Тестовый тест");
        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(workspace.Id);
        result.Name.Should().Be(command.Name);

        var workspaceDb = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspace.Id, CancellationToken.None);
        workspaceDb.Should().NotBeNull();
        workspaceDb.Name.ToString().Should().Be(command.Name);
    }
}
