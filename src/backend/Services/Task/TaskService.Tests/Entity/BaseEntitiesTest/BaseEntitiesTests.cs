namespace TaskService.Tests.Entity.BaseEntitiesTest;

public class BaseEntitiesTests
{
    [Fact]
    public void Valid_CreateDate()
    {
        DateTime created = DateTime.UtcNow;
        BaseEntitiesChild child = new BaseEntitiesChild(Guid.CreateVersion7(), "Test", created);
        Assert.Equal(child.CreatedDate, created);
    }
}
