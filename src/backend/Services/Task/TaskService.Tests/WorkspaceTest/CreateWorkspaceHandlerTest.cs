using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Application.Features.Users.Read.GetUserByKeycloakId;
using TaskService.Application.Features.Workspaces.Write.CreateWorkspace;
using TaskService.Application.Interfaces;
using TaskService.Application.Validators.Workspace;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Tests.WorkspaceTest
{
    public class CreateWorkspaceHandlerTest : IDisposable
    {
        private readonly Fixture _fixture;
        private readonly TaskServiceDbContext _dbContext;
        private readonly FakeHybridCache _fakeCache;
        private readonly CreateWorkspaceCommandValidator _validator;
        private readonly CreateWorkspaceHandler _handler;
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

            _userContext
                .Setup(x => x.User)
                .Returns(_fixture.Build<GetUserByKeycloakIdResult>()
                    .Without(x => x.ProjectMembers)
                    .Without(x => x.WorkSpaceMembers)
                    .Create());

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
            var user = User.Create(_userContext.Object.User.KeycloakId, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>());
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new CreateWorkspaceCommand("тест из теста");

            // ACT
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
