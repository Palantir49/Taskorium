using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Projects.Read.GetProjectMembers;
using TaskService.Application.Mapping;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest.Read;

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

        _fixture.Register(() => User.Create(
            keycloakId: _fixture.Create<Guid>(),
            userName: _fixture.Create<string>(),
            email: _fixture.Create<string>(),
            fullName: _fixture.Create<string>()));

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

    [Fact]
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

    [Fact]
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
        result.Members.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetProjectMembersFromDb_WhenProjectAndProjectMembersExists_ReturnResult()
    {
        // ARRANGE
        Guid guid = _fixture.Create<Guid>();

        var proj1 = _fixture.Create<Project>();
        var proj2 = _fixture.Create<Project>();
        _context.Projects.AddRange(proj1, proj2);

        var user1 = _fixture.Create<User>(); //1
        var user2 = _fixture.Create<User>(); //1 2
        var user3 = _fixture.Create<User>(); //2
        var user4 = _fixture.Create<User>(); // нигде

        _context.Users.AddRange(user1, user2, user3, user4);

        var member1 = ProjectMember.Create(
            projectId: proj1.Id,
            userId: user1.Id,
            role: Domain.Entities.Enums.ProjectRoles.Admin,
            joinedAt: _fixture.Create<DateTimeOffset>());

        var member2 = ProjectMember.Create(
            projectId: proj1.Id,
            userId: user2.Id,
            role: Domain.Entities.Enums.ProjectRoles.Viewer,
            joinedAt: _fixture.Create<DateTimeOffset>());

        var member3 = ProjectMember.Create(
            projectId: proj2.Id,
            userId: user2.Id,
            role: Domain.Entities.Enums.ProjectRoles.Admin,
            joinedAt: _fixture.Create<DateTimeOffset>());

        var member4 = ProjectMember.Create(
            projectId: proj2.Id,
            userId: user3.Id,
            role: Domain.Entities.Enums.ProjectRoles.Admin,
            joinedAt: _fixture.Create<DateTimeOffset>());

        _context.ProjectMembers.AddRange(member1, member2, member3, member4);

        await _context.SaveChangesAsync(CancellationToken.None);
        //ACT
        var result = await _handler.GetProjectMembersFromDbAsync(proj1.Id, CancellationToken.None);

        // ASSERT
        result.ProjectId.Should().Be(proj1.Id);
        result.ProjectName.Should().Be(proj1.Name.ToString());
        result.Members.Should().HaveCount(2);

        Guid[] guids = { user1.Id, user2.Id };
        result.Members.Select(x => x.Id).Should().BeEquivalentTo(guids);

        var memberRes1 = result.Members.First(x => x.Id == user1.Id);
        memberRes1.KeycloakId.Should().Be(user1.KeycloakId);
        memberRes1.Email.Should().Be(user1.Email.ToString());
        memberRes1.UserName.Should().Be(user1.Username.ToString());
        memberRes1.JoinedAt.Should().Be(member1.JoinedAt);
        memberRes1.Role.Should().Be(member1.Role.ToDto());



        var memberRes2 = result.Members.First(x => x.Id == user2.Id);
        memberRes2.KeycloakId.Should().Be(user2.KeycloakId);
        memberRes2.Email.Should().Be(user2.Email.ToString());
        memberRes2.UserName.Should().Be(user2.Username.ToString());
        memberRes2.JoinedAt.Should().Be(member2.JoinedAt);
        memberRes2.Role.Should().Be(member2.Role.ToDto());
    }
}
