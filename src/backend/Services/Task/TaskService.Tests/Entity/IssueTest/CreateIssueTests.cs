using TaskService.Domain.Entities;

namespace TaskService.Tests.Entity.IssueTest
{
    public class CreateIssueTests
    {
        [Fact]
        public void Create_WithValidData_SetsAllPropertiesCorrectly()
        {
            Guid projectId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef");
            Guid typeId = Guid.Parse("b2c3d4e5-e5f6-7890-1234-567890abcdef");
            Guid statusId = Guid.Parse("c3d4e5f6-e5f6-7890-1234-567890abcdef");

            Issue issue = Issue.Create("Task", "Desc", projectId, typeId, statusId, null, null);

            Assert.Equal("Task", issue.Name.Value);
            Assert.Equal("Desc", issue.Description);
            Assert.Equal(projectId, issue.ProjectId);
            Assert.Equal(typeId, issue.TaskTypeId);
            Assert.Equal(statusId, issue.TaskStatusId);
            Assert.Null(issue.ReporterId);
            Assert.Null(issue.ResolvedDate);
            Assert.Null(issue.UpdatedDate);
            Assert.NotEqual(default, issue.CreatedDate);
        }

        [Fact]
        public void Create_WithReporter_ValidReporterId()
        {
            Guid guid = Guid.Parse("019b659f-0a03-7fe3-be92-5b162c2c824c");
            Issue issue = Issue.Create("Old", "", Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), guid, null);
            Assert.NotNull(issue.ReporterId);
            Assert.Equal(guid, issue.ReporterId);
        }

        [Fact]
        public void Create_WithoutReporter_NullReporterId()
        {
            Issue issue = Issue.Create("Old", "", Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), null, null);
            Assert.Null(issue.ReporterId);
        }
    }
}
