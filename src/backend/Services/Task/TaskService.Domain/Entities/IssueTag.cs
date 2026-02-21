using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class IssueTag : BaseEntities
{
    public Guid ProjectId { get; }
    public string? Color { get; private set; }

    protected IssueTag() { }

    private IssueTag(Guid id, string name, Guid projectId, string? color) : base(id, name)
    {
        ProjectId = projectId;
        Color = color;
    }

    public static IssueTag Create(string name, Guid projectId, string? color)
    {
        return new IssueTag(
            id: Guid.CreateVersion7(),
            name: name,
            projectId:
            projectId,
            color: color);
    }

    public void UpdateColor(string? color) => Color = color;
}
