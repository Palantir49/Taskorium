using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class IssueType : BaseEntities
{
    public Guid ProjectId { get; }
    public string? Color { get; private set; }

    protected IssueType() { }

    private IssueType(Guid id, string name, Guid projectId, string color):base(id, name)
    {
        ProjectId = projectId;
        Color = color;
    }

    public static IssueType Create(string name, Guid projectId, string color)
    {
        return new IssueType(
            id: Guid.CreateVersion7(), 
            name: name, 
            projectId: 
            projectId, 
            color: color);
    }

}
