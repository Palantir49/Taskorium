using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Application.Features.Users.Read.GetUserByKeycloakId;
using TaskService.Application.Features.Workspaces.Write.CreateWorkspace;
using TaskService.Application.Interfaces;
using TaskService.Application.Validators.Workspace;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.WorkspaceTest
{
    public class CreateWorkspaceHandlerTest : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly TaskServiceDbContext _dbContext;
        private readonly FakeHybridCache _fakeCache;
        private readonly CreateWorkspaceCommandValidator _validator;
        private readonly CreateWorkspaceHandler _handler;
        private readonly User _user;
        private readonly Mock<ICurrentUserContext> _userContext;
        private bool _disposed;

        public CreateWorkspaceHandlerTest()
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

            _validator = new CreateWorkspaceCommandValidator();

            _fakeCache = new FakeHybridCache();

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

            _handler = new CreateWorkspaceHandler(_dbContext, _fakeCache, _userContext.Object, _validator);
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

        public static IEnumerable<object[]> InvalidCommandData()
        {
            yield return new object[] {
                new CreateWorkspaceCommand(string.Empty)
            };

            yield return new object[] {
                new CreateWorkspaceCommand("%^&*()")
            };

            yield return new object[] {
                new CreateWorkspaceCommand("йwertyuiopйwertyuiopйwertyuiopйwertyuiopйwertyuiopйwertyuiop")
            };
        }

        [Theory]
        [MemberData(nameof(InvalidCommandData))]
        public async Task CreateWorkspaceHandler_WhenValidationFails_ThrowsValidationException(CreateWorkspaceCommand command)
        {
            // ARRANGE
            // ACT
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            var exception = await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task CreateWorkspaceHandler_WhenUserNotExist_ThrowsKeyNotFoundExceptionn()
        {
            // ARRANGE
            var command = new CreateWorkspaceCommand("тест из теста");
            // ACT
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CreateWorkspaceHandler_WhenUserAndWorkspaceValid_AddsWorkspaceAndMembersAndReturnsResult()
        {
            // ARRANGE
            _dbContext.Users.Add(_user);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new CreateWorkspaceCommand("тест из теста ");

            // ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name.Trim());
            result.Role.Should().Be(Contracts.Enum.WorkspaceRolesDto.Creator);

            var workspace = await _dbContext.Workspaces.Include(x => x.WorkspaceMembers).FirstAsync(x => x.Id == result.Id, CancellationToken.None);
            workspace.Name.ToString().Should().Be(command.Name.Trim());
            workspace.WorkspaceMembers.Should().HaveCount(1);
            workspace.WorkspaceMembers.First(x => x.WorkspaceId == result.Id).UserId.Should().Be(_user.Id);
        }
    }
}
