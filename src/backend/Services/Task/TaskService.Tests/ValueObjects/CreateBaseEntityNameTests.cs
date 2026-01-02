using System.Xml.Linq;
using TaskService.Domain.ValueObjects;

namespace TaskService.Tests.ValueObjects;

public class CreateBaseEntityNameTests
{
    [Theory]
    [InlineData("Test")]
    [InlineData("  SpaceTest  ")]
    public void BaseEntityName_CreateValidName_TrimedAndValid(string name)
    {
        BaseEntityName VOName = new BaseEntityName(name);
        Assert.NotNull(VOName);
        Assert.Equal(VOName.ToString(), name.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void BaseEntityName_CreateEmptyName_ArgumentNullException(string name)
    {
        Assert.Throws<ArgumentNullException>(() => new BaseEntityName(name));
    }
}
