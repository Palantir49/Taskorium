using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Projects.Read.GetProjectMembers;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest;

public class GetProjectMembersFromDbTest : IDisposable
{
    private readonly Fixture _fixture;
    private bool _disposed;

    private readonly TaskServiceDbContext _context;
    private readonly FakeHybridCache _fakeCache;
    private readonly GetProjectMembersHandler _handler;

    public GetProjectMembersFromDbTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() =>
            Project.Create(
                name: _fixture.Create<string>(),
                description: _fixture.Create<string>(),
                abbreviation: "ttt",
                workspaceId: _fixture.Create<Guid>()
                ));

        var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new TaskServiceDbContext(options);

        _fakeCache = new FakeHybridCache();

        _handler = new GetProjectMembersHandler(_context, _fakeCache);
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

    public async Task GetProjectMembersFromDb_WhenProjectNotExist_TrowKeyNotFoundException()
    {
        // ARRANGE
        Guid guid = _fixture.Create<Guid>();

        var proj1 = _fixture.Create<Project>();
        var proj2 = _fixture.Create<Project>();
        var proj3 = _fixture.Create<Project>();
        var proj4 = _fixture.Create<Project>();

        _context.Projects.AddRange(proj1, proj2, proj3, proj4);
        await _context.SaveChangesAsync(CancellationToken.None);
        //ACT
        Func<Task> act = async () => await _handler.GetProjectMembersFromDbAsync(guid, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{guid} не найден*");
    }

    public async Task GetProjectMembersFromDb_WhenProjectExistAndProjectMembersNotExist_ReturnEmptyResult()
    {
        // ARRANGE
        Guid guid = _fixture.Create<Guid>();

        var proj1 = _fixture.Create<Project>();
        var proj2 = _fixture.Create<Project>();
        var proj3 = _fixture.Create<Project>();
        var proj4 = _fixture.Create<Project>();

        _context.Projects.AddRange(proj1, proj2, proj3, proj4);
        await _context.SaveChangesAsync(CancellationToken.None);
        //ACT
        var result = await _handler.GetProjectMembersFromDbAsync(proj1.Id, CancellationToken.None);

        // ASSERT
        result.ProjectId.Should().Be(proj1.Id);
        result.ProjectName.Should().Be(proj1.Name.ToString());
        result.Members.
    }
}
