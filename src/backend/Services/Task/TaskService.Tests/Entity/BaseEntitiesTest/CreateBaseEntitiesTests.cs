namespace TaskService.Tests.Entity.BaseEntitiesTest;

public class CreateBaseEntitiesTests
{
    [Fact]
    public void Create_WithValidData_CreateDateApproximatelyNow()
    {
        BaseEntitiesTestChild child = new BaseEntitiesTestChild(Guid.CreateVersion7(), "Test");
        DateTimeOffset now = DateTimeOffset.UtcNow; 
        Assert.True(child.CreatedDate <= now);
        Assert.True(child.CreatedDate >= now.AddSeconds(-1));
        Assert.NotEqual(default, child.CreatedDate);
    }
}
