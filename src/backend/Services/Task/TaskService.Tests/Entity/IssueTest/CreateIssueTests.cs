using System.Xml.Linq;
using TaskService.Domain.Entities;
namespace TaskService.Tests.Entity.IssueTest;


public class CreateIssueTests
{
    [Theory]
    [InlineData("Test")]
    [InlineData("  SpaceTest  ")]
    public void Valid_Create_Issue(string name)
    {
        Issue issue = Issue.Create(name, "", Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), null);
        Assert.NotNull(issue);
        Assert.Equal(issue.Name.ToString(), name.Trim());

    }

    [Fact]
    public void Valid_Null_ReporterId_Issue()
    {
        Issue issue = Issue.Create("Test", "", Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), null, null);
        Assert.Null(issue.ReporterId);
    }
}
