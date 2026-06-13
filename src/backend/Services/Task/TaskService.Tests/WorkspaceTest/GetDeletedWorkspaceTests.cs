using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Workspaces.Read.GetDeletedWorkspace;
using TaskService.Application.Features.Workspaces.Read.GetWorkspaceMembers;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest
{
    public class GetDeletedWorkspaceTests : IDisposable
    {
        //TODO: хз как это тестить
        //private readonly Fixture _fixture;
        //private readonly TaskServiceDbContext _context;
        //private readonly GetDeletedWorkspaceHandler _handler;

        //public GetDeletedWorkspaceTests()
        //{
        //    _fixture = new Fixture();

        //    // Настраиваем AutoFixture так, чтобы он не пытался создавать реальные подключения к БД
        //    _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        //        .ForEach(b => _fixture.Behaviors.Remove(b));
        //    _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        //    var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
        //            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
        //            .Options;

        //    _context = new TaskServiceDbContext(options);

        //    _fixture.Register(() => Workspace.Create(_fixture.Create<string>()));

        //    // Инициализируем хэндлер с замоканной зависимостью
        //    _handler = new GetDeletedWorkspaceHandler(_context);
        //}

        //[Fact]
        //public async Task GetDeletedWorkspace_WhenRemoteWorkspace_ReturnsCorrectResult()
        //{
        //    // ARRANGE
        //    var workspaceDelete1 = _fixture.Create<Workspace>();
        //    var workspaceDelete2 = _fixture.Create<Workspace>();
        //    var workspaceLife = _fixture.Create<Workspace>();

        //    _context.Workspaces.AddRange(workspaceDelete1, workspaceDelete2, workspaceLife);
        //    await _context.SaveChangesAsync(CancellationToken.None);

        //    _context.Workspaces.Remove(workspaceDelete1);
        //    _context.Workspaces.Remove(workspaceDelete2);
        //    await _context.SaveChangesAsync(CancellationToken.None);

        //    //ACT
        //    var result = await _handler.Handle(new GetDeletedWorkspacePageQuery(0, 5), CancellationToken.None);

        //    // ASSERT
        //    result.Workspaces.Should().HaveCount(2);

        //    Guid[] ids = new[] { workspaceDelete1.Id, workspaceDelete2.Id };
        //    result.Workspaces.Select(x => x.Id).Should().BeEquivalentTo(ids);
        //}


        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}
