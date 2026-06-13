using System.ComponentModel.DataAnnotations;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Application.Features.Projects.Read.GetProjectById;
using TaskService.Application.Features.Users.Read.GetUserByKeycloakId;
using TaskService.Application.Interfaces;
using TaskService.Application.Mapping;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Tests.ProjectTest.Read
{
    public class ProjectGetByIdFromDbrTest : IDisposable
    {
        private readonly Fixture _fixture;
        private bool _disposed;

        private readonly TaskServiceDbContext _dbContext;
        private readonly FakeHybridCache _fakeCache;
        private readonly ProjectGetByIdHandler _handler;
        private readonly User _user;
        private readonly Mock<ICurrentUserContext> _userContext;

        public ProjectGetByIdFromDbrTest()
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

            _dbContext = new TaskServiceDbContext(options);
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

            _handler = new ProjectGetByIdHandler(_dbContext, _fakeCache, _userContext.Object);
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
        public async Task GetProjectByIdFromDb_WhenProjectNotExist_ThrowKeyNotFoundException()
        {
            // ARRANGE
            Guid guid = Guid.NewGuid();

            //ACT
            Func<Task> act = async () => await _handler.GetProjectByIdFromDbAsync(guid, CancellationToken.None);

            // ASSERT
            var exception = await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{guid} не найден*");
        }

        [Fact]
        public async Task GetProjectByIdFromDb_WhenProjectExistsAndProjectMembersNotExists_ThrowValidationException()
        {
            // ARRANGE
            var project = _fixture.Create<Project>();
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            //ACT
            Func<Task> act = async () => await _handler.GetProjectByIdFromDbAsync(project.Id, CancellationToken.None);

            // ASSERT
            var exception = await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Проект не содержит пользователей");
        }

        [Fact]
        public async Task GetProjectByIdFromDb_WhenProjectExistsAndProjectMembersNotValid_ThrowValidationException()
        {
            // ARRANGE
            var project = _fixture.Create<Project>();
            _dbContext.Projects.Add(project);

            var members = ProjectMember.Create(
                projectId: project.Id, 
                userId: Guid.NewGuid(), 
                role: Domain.Entities.Enums.ProjectRoles.Admin, 
                joinedAt: _fixture.Create<DateTimeOffset>());

            _dbContext.Projects.Add(project);
            _dbContext.ProjectMembers.Add(members);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            //ACT
            Func<Task> act = async () => await _handler.GetProjectByIdFromDbAsync(project.Id, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Пользователь не имеет доступа к проекту");
        }

        [Fact]
        public async Task GetProjectByIdFromDb_WhenProjectExistsAndProjectMembersExists_ReturnResult()
        {
            // ARRANGE
            var project = _fixture.Create<Project>();
            _dbContext.Projects.Add(project);

            var members = ProjectMember.Create(
                projectId: project.Id,
                userId: _user.Id,
                role: Domain.Entities.Enums.ProjectRoles.Admin,
                joinedAt: _fixture.Create<DateTimeOffset>());

            _dbContext.Projects.Add(project);
            _dbContext.ProjectMembers.Add(members);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            //ACT
            var result = await _handler.GetProjectByIdFromDbAsync(project.Id, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.Name.Should().Be(project.Name.ToString());
            result.Description.Should().Be(project.Description);
            result.Abbreviation.Should().Be(project.Abbreviation);
            result.CreatedDate.Should().Be(project.CreatedDate);
            result.WorkspaceId.Should().Be(project.WorkspaceId);

            result.Role.Should().Be(members.Role.ToDto());
        }
    }
}
