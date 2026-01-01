using TaskService.Domain.ValueObjects;

namespace TaskService.Tests.ValueObjects;

public class BaseEntityNameTest
{
    [Theory]
    [InlineData("Test")]
    [InlineData("  SpaceTest  ")]
    public void Valid_Create(string name)
    {
        BaseEntityName VOName = new BaseEntityName(name);
        Assert.NotNull(VOName);
        Assert.Equal(VOName.ToString(), name.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Not_Valid_Create(string name)
    {
        var act = () => new BaseEntityName(name);
        Assert.Throws<ArgumentNullException>(act);
    }
}
