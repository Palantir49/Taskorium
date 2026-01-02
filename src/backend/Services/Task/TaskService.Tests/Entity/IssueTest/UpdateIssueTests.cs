using System.ComponentModel;
using System.Runtime.Intrinsics.X86;
using TaskService.Domain.Entities;

namespace TaskService.Tests.Entity.IssueTest;

public class UpdateIssueTests
{
    public Issue Issue { get; set; }

    public UpdateIssueTests()
    {
        Issue = Issue.Create("Old", "", Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), null);
    }

    [Theory]
    [InlineData("new")]
    [InlineData("   space    ")]
    public void Update_WithValidName_UpdateNameAndUpdateDate(string name)
    {
        DateTimeOffset? old = Issue.UpdatedDate;

        Issue.UpdateName(name);

        Assert.Equal(Issue.Name.ToString(), name.Trim());
        CheckChangeUpdateDate(old, Issue.UpdatedDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_WithEmptyName_UpdateNameAndUpdateDate(string name)
    {
        Assert.Throws<ArgumentNullException>(() => Issue.UpdateName(name));
    }

    [Fact]
    public void Update_WithChangeThePropertyTwice_UpdateDateChangeTwice()
    {
        DateTimeOffset? old = Issue.UpdatedDate;

        Issue.UpdateName("Two");

        CheckChangeUpdateDate(old, Issue.UpdatedDate);
        old = Issue.UpdatedDate;

        Issue.UpdateName("Three");

        CheckChangeUpdateDate(old, Issue.UpdatedDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("тест")]
    [InlineData(" тест с пробелами   ")]
    public void Update_WithDescription_ChangeDescriptionAndUpdateDate(string? description)
    {
        DateTimeOffset? old = Issue.UpdatedDate;

        Issue.UpdateDescription(description);

        Assert.Equal(description?.Trim(), Issue.Description);
        CheckChangeUpdateDate(old, Issue.UpdatedDate);
    }

    [Fact]
    public void Update_WithValidDueDate_ChangeDueDateAndUpdateDate()
    {
        DateTimeOffset date = DateTimeOffset.UtcNow;
        DateTimeOffset? old = Issue.UpdatedDate;

        Issue.UpdateDueDate(date);

        Assert.NotEqual(old, Issue.DueDate);
        Assert.NotEqual(default, Issue.DueDate);
        Assert.Equal(date, Issue.DueDate);
    }

    //TODO: ChangeStatus

    //TODO: UpdateType



    private void CheckChangeUpdateDate(DateTimeOffset? oldDate, DateTimeOffset? newDate)
    {
        Assert.NotEqual(oldDate, newDate);
        if (oldDate.HasValue)
            Assert.True(oldDate < newDate);
    }
}
