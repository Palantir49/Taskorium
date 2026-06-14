using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Projects.Write.CreateProject;
using TaskService.Application.Validators.Project;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest.Write;

public class CreateProjectHandlerTest : IDisposable
{
    private readonly Fixture _fixture;
    private readonly TaskServiceDbContext _context;
    private readonly FakeHybridCache _fakeCache;
    private readonly CreateProjectCommandValidator _validator;
    private readonly CreateProjectHandler _handler;
    private bool _disposed;

    public CreateProjectHandlerTest()
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

        _validator = new CreateProjectCommandValidator();

        _fakeCache = new FakeHybridCache();

        _handler = new CreateProjectHandler(_context, _fakeCache, _validator);
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
        string test100Symbol = "йцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщзйцукенгшщз";

        //Name
        yield return new object[] {
        new CreateProjectCommand(
            Name: "",
            Description: "test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        yield return new object[] {
        new CreateProjectCommand(
            Name: "!@#$$%%",
            Description: "test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        yield return new object[] {
        new CreateProjectCommand(
            Name: string.Concat(Enumerable.Repeat(test100Symbol, 3)),
            Description: "test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        //Description
        yield return new object[] {
        new CreateProjectCommand(
            Name: "Test",
            Description: string.Concat(Enumerable.Repeat(test100Symbol, 21)),
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        //Abbreviation
        yield return new object[] {
        new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        yield return new object[] {
        new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: test100Symbol,
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid())
        };

        //WorkspaceId
        yield return new object[] {
        new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.Empty,
            UserId: Guid.NewGuid())
        };

        //UserId
        yield return new object[] {
        new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.Empty)
        };
    }

    [Theory]
    [MemberData(nameof(InvalidCommandData))]
    public async Task CreateProjectHandler_WhenValidationFails_ThrowsValidationException(CreateProjectCommand command)
    {
        // ARRANGE

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateProjectHandler_WhenWorlspaceNotExist_TrowKeyNotFoundException()
    {
        // ARRANGE
        var command = new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "ttt",
            WorkspaceId: Guid.NewGuid(),
            UserId: Guid.NewGuid());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*id: {command.WorkspaceId} не найдена*");
    }

    [Fact]
    public async Task CreateProjectHandler_WhenUserNotExist_TrowKeyNotFoundException()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "ttt",
            WorkspaceId: ws.Id,
            UserId: Guid.NewGuid());

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        var exception = await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Пользователь с  id {command.UserId} не существует");
    }

    [Fact]
    public async Task CreateProjectHandler_AllDataValid_CreateProjectAndReturnResult()
    {
        // ARRANGE
        var ws = _fixture.Create<Workspace>();
        _context.Workspaces.Add(ws);

        var user = _fixture.Create<User>();
        _context.Users.Add(user);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new CreateProjectCommand(
            Name: "Test",
            Description: "test test",
            Abbreviation: "ttt",
            WorkspaceId: ws.Id,
            UserId: user.Id);

        DateTimeOffset time = DateTimeOffset.UtcNow;

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Abbreviation.Should().Be(command.Abbreviation);
        result.WorkspaceId.Should().Be(command.WorkspaceId);
        result.Role.Should().Be(ProjectRolesDto.Creator);
        result.CreatedDate.Should().BeOnOrAfter(time).And.BeOnOrBefore(time.AddSeconds(5));
    }
}

