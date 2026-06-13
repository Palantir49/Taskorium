using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Workspaces.Write.DeleteWorkspaceById;
using TaskService.Application.Validators.WorkspaceMember;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest.Write;

public class DeleteWorkspaceByIdHandlerTest : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _dbContext;
    private readonly FakeHybridCache _fakeCache;
    private readonly AddWorkspaceMemberCommandValidator _validator;
    private readonly DeleteWorkspaceByIdHandler _handler;
    private bool _disposed;

    public DeleteWorkspaceByIdHandlerTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() => Workspace.Create(_fixture.Create<string>()));

        var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new TaskServiceDbContext(options);

        _validator = new AddWorkspaceMemberCommandValidator();

        _fakeCache = new FakeHybridCache();

        _handler = new DeleteWorkspaceByIdHandler(_dbContext, _fakeCache);
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
    public async Task DeleteWorkspaceByIdHandler_WhenWorkspaceNotExist_TrowArgumentException()
    {
        // ARRANGE
        var command = new DeleteWorkspaceByIdCommand(Guid.NewGuid());
        //ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteWorkspaceByIdHandler_WhenValidWorkspace_DeleteWorkspaceAndReturnResult()
    {
        // ARRANGE
        var workspace = _fixture.Create<Workspace>();
        _dbContext.Workspaces.Add(workspace);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var command = new DeleteWorkspaceByIdCommand(workspace.Id);
        //ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();

        var workspaceDb = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == workspace.Id, CancellationToken.None);
        workspaceDb.Should().BeNull();
    }
}
