using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class IssueTag : BaseEntities
{
    public Guid ProjectId { get; }

    public List<Issue> Issues { get; private set; } = new();

    protected IssueTag() { }

    private IssueTag(Guid id, string name, Guid projectId) : base(id, name)
    {
        ProjectId = projectId;
    }

    public static IssueTag Create(string name, Guid projectId)
    {
        return new IssueTag(
            id: Guid.CreateVersion7(),
            name: name,
            projectId:
            projectId);
    }
}
