using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Application.Features.Projects.Read.GetProjectByWorkspaceId;
using TaskService.Application.Features.Users.Read.GetUserByKeycloakId;
using TaskService.Application.Interfaces;
using TaskService.Application.Mapping;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest.Read;

public class GetProjectByWorkspaceIdFromDbTest : IDisposable
{
    private readonly Fixture _fixture;
    private bool _disposed;

    private readonly TaskServiceDbContext _context;
    private readonly FakeHybridCache _fakeCache;
    private readonly GetProjectByWorkspaceIdHandler _handler;
    private readonly User _user;
    private readonly Mock<ICurrentUserContext> _userContext;

    public GetProjectByWorkspaceIdFromDbTest()
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
        _userContext = _fixture.Freeze<Mock<ICurrentUserContext>>();

        _userContext
            .Setup(x => x.IsInitialized)
            .Returns(true);

        _user = User.Create(
            keycloakId: Guid.CreateVersion7(),
            userName: "nenene",
            email: "memememe",
            fullName: "neneneMEMEME");

        _userContext
            .Setup(x => x.User)
            .Returns(new GetUserByKeycloakIdResult(Id: _user.Id, KeycloakId: _user.KeycloakId, ProjectMembers: null, WorkSpaceMembers: null));

        _fakeCache = new FakeHybridCache();

        _handler = new GetProjectByWorkspaceIdHandler(_context, _fakeCache, _userContext.Object);
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

    [Fact]
    public async Task GetProjectGetProjectByWorkspaceIdFromDb_WhenWorkspaceIdNotExist_ReturnEmptyResult()
    {
        // ARRANGE
        var workspace = Workspace.Create("Тестовый тест");
        _context.Workspaces.Add(workspace);

        var project1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttБ",
            workspaceId: workspace.Id);

        var project2 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttА",
            workspaceId: workspace.Id);

        _context.Projects.AddRange(project1, project2);

        await _context.SaveChangesAsync(CancellationToken.None);
        //ACT
        var result = await _handler.GetProjectByWorkspaceIdFromDb(Guid.NewGuid(), CancellationToken.None);

        // ASSERT
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetProjectGetProjectByWorkspaceIdFromDb_WhenWorkspaceIdExistAndProjectMembersNotExists_ReturnEmptyResult()
    {
        // ARRANGE
        var workspace = Workspace.Create("Тестовый тест");
        _context.Workspaces.Add(workspace);

        var project1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttБ",
            workspaceId: workspace.Id);

        var project2 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttА",
            workspaceId: workspace.Id);

        _context.Projects.AddRange(project1, project2);

        await _context.SaveChangesAsync(CancellationToken.None);

        //ACT
        var result = await _handler.GetProjectByWorkspaceIdFromDb(workspace.Id, CancellationToken.None);

        // ASSERT
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetProjectGetProjectByWorkspaceIdFromDb_WhenWorkspaceIdAndProjectMembersExists_ReturnResult()
    {
        // ARRANGE
        var workspace = Workspace.Create("Тестовый тест");
        _context.Workspaces.Add(workspace);

        var project1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttБ",
            workspaceId: workspace.Id);

        var project2 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "tttА",
            workspaceId: workspace.Id);

        _context.Projects.AddRange(project1, project2);

        var member1 = ProjectMember.Create(
            projectId: project1.Id,
            userId: _user.Id,
            role: Domain.Entities.Enums.ProjectRoles.Creator,
            joinedAt: _fixture.Create<DateTimeOffset>());

        _context.ProjectMembers.Add(member1);

        await _context.SaveChangesAsync(CancellationToken.None);

        //ACT
        var result = await _handler.GetProjectByWorkspaceIdFromDb(workspace.Id, CancellationToken.None);

        // ASSERT
        result.Should().HaveCount(1);

        foreach (var res in result)
        {
            res.Id.Should().Be(project1.Id);
            res.Name.Should().Be(project1.Name.ToString());
            res.Description.Should().Be(project1.Description);
            res.Abbreviation.Should().Be(project1.Abbreviation);
            res.WorkspaceId.Should().Be(project1.WorkspaceId);
            res.CreatedDate.Should().Be(project1.CreatedDate);
            res.Role.Should().Be(member1.Role.ToDto());
        }
    }
}
