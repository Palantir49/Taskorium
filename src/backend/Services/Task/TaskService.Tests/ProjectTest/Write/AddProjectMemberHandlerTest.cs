using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.Projects.Write.AddProjectMember;
using TaskService.Application.Mapping;
using TaskService.Application.Validators.ProjectMember;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest.Write;

public class AddProjectMemberHandlerTest : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _context;
    private readonly FakeHybridCache _fakeCache;
    private readonly AddProjectMemberCommandValidator _validator;
    private readonly AddProjectMemberHandler _handler;
    private bool _disposed;

    public AddProjectMemberHandlerTest()
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

        _context = new TaskServiceDbContext(options);

        _validator = new AddProjectMemberCommandValidator();

        _fakeCache = new FakeHybridCache();

        _handler = new AddProjectMemberHandler(_context, _fakeCache, _validator);
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
        new AddProjectMemberCommand(
            ProjectId: Guid.Empty,
            UserId: Guid.NewGuid(),
            RoleDto: ProjectRoles.Admin.ToDto())
    };
        yield return new object[] {
        new AddProjectMemberCommand(
            ProjectId: Guid.NewGuid(),
            UserId: Guid.Empty,
            RoleDto: ProjectRoles.Admin.ToDto())
    };
    }

    [Theory]
    [MemberData(nameof(InvalidCommandData))]
    public async Task AddProjectMemberHandler_WhenValidationFails_ThrowsValidationException(AddProjectMemberCommand command)
    {
        // ARRANGE

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AddProjectMemberHandler_WhenProjectNotExist_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var command = new AddProjectMemberCommand(
            ProjectId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            RoleDto: ProjectRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{command.ProjectId} не существует*");
    }

    [Fact]
    public async Task AddProjectMemberHandler_WhenProjectExistAndUserNotExist_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var proj1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        _context.Projects.Add(proj1);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new AddProjectMemberCommand(
            ProjectId: proj1.Id,
            UserId: Guid.NewGuid(),
            RoleDto: ProjectRoles.Admin.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{command.UserId} не существует*");
    }

    [Fact]
    public async Task AddProjectMemberHandler_WhenProjectAndUserExist_AndWorkspaceMemberNotExist_ThrowsKeyNotFoundException()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var proj1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        _context.Projects.Add(proj1);

        var user1 = _fixture.Create<User>();
        _context.Users.AddRange(user1);

        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new AddProjectMemberCommand(
            ProjectId: proj1.Id,
            UserId: user1.Id,
            RoleDto: ProjectRoles.Member.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*рабочей области с id {ws.Id}*");
    }

    [Fact]
    public async Task AddProjectMemberHandler_AllDataValidAndProjectMemberExist_ThrowsConflictException()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var proj1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        _context.Projects.Add(proj1);

        var user1 = _fixture.Create<User>();
        _context.Users.Add(user1);

        var projMember1 = ProjectMember.Create(
            projectId: proj1.Id,
            userId: user1.Id,
            role: ProjectRoles.Admin,
            joinedAt: _fixture.Create<DateTimeOffset>());

        _context.ProjectMembers.Add(projMember1);

        var wsMember1 = WorkspaceMember.Create(
            workspaceId: ws.Id,
            userId: user1.Id,
            role: WorkspaceRoles.Viewer);

        _context.WorkspaceMembers.Add(wsMember1);

        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new AddProjectMemberCommand(
            ProjectId: proj1.Id,
            UserId: user1.Id,
            RoleDto: ProjectRoles.Member.ToDto());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage($"Пользователь уже состоит в проекте");
    }

    [Fact]
    public async Task AddProjectMemberHandler_AllDataValid_AddedProjectMemberAndReturnResult()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var proj1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        _context.Projects.Add(proj1);

        var user1 = _fixture.Create<User>();
        _context.Users.Add(user1);

        var wsMember1 = WorkspaceMember.Create(
            workspaceId: ws.Id,
            userId: user1.Id,
            role: WorkspaceRoles.Viewer);

        _context.WorkspaceMembers.Add(wsMember1);

        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new AddProjectMemberCommand(
            ProjectId: proj1.Id,
            UserId: user1.Id,
            RoleDto: ProjectRoles.Member.ToDto());

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.UserId.Should().Be(user1.Id);
        result.ProjectId.Should().Be(proj1.Id);
        result.RoleDto.Should().Be(ProjectRoles.Member.ToDto());

        var members = await _context.ProjectMembers.ToListAsync(CancellationToken.None);

        members.Should().HaveCount(1);
        members[0].ProjectId.Should().Be(proj1.Id);
        members[0].UserId.Should().Be(user1.Id);
        members[0].Role.Should().Be(command.RoleDto.ToEntity());
    }

    [Fact]
    public async Task AddProjectMemberHandler_AllDataValidAndProjectMembersNotEmpty_AddedProjectMemberWithoutChangingTheOtherProjectMembers()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var proj1 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        var proj2 = Project.Create(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            abbreviation: "ttt",
            workspaceId: ws.Id
            );

        _context.Projects.AddRange(proj1, proj2);

        var user1 = _fixture.Create<User>();
        var user2 = _fixture.Create<User>();
        var user3 = _fixture.Create<User>();
        _context.Users.AddRange(user1, user2, user3);

        var member1 = ProjectMember.Create(
            projectId: proj1.Id,
            userId: user1.Id,
            role: ProjectRoles.Admin,
            joinedAt: _fixture.Create<DateTimeOffset>());

        var member2 = ProjectMember.Create(
            projectId: proj1.Id,
            userId: user2.Id,
            role: ProjectRoles.Viewer,
            joinedAt: _fixture.Create<DateTimeOffset>());

        var member3 = ProjectMember.Create(
            projectId: proj2.Id,
            userId: user2.Id,
            role: ProjectRoles.Member,
            joinedAt: _fixture.Create<DateTimeOffset>());

        _context.ProjectMembers.AddRange(member1, member2, member3);

        var wsMember1 = WorkspaceMember.Create(
            workspaceId: ws.Id,
            userId: user3.Id,
            role: WorkspaceRoles.Viewer);

        _context.WorkspaceMembers.Add(wsMember1);

        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new AddProjectMemberCommand(
            ProjectId: proj1.Id,
            UserId: user3.Id,
            RoleDto: ProjectRoles.Member.ToDto());

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.UserId.Should().Be(user3.Id);
        result.ProjectId.Should().Be(proj1.Id);
        result.RoleDto.Should().Be(ProjectRoles.Member.ToDto());

        var membersDb = await _context.ProjectMembers.ToListAsync(CancellationToken.None);

        membersDb.Should().HaveCount(4);

        // Проверяем, что новый участник создан правильно
        var newMember = membersDb.Single(x => x.UserId == user3.Id);
        newMember.ProjectId.Should().Be(proj1.Id);
        newMember.Role.Should().Be(command.RoleDto.ToEntity());
    }
}
