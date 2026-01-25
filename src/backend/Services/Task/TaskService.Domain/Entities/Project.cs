using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Project : BaseEntities
{
    protected Project() { }

    private Project(Guid id, string name, string? description, Guid workspaceId)
        : base(id, name)
    {
        WorkspaceId = workspaceId;
        Description = description;
    }
    public Guid WorkspaceId { get; }

    public string? Description { get; private set; }

    public DateTimeOffset? StartDate { get; private set; }

    public DateTimeOffset? FinishDate { get; private set; }

    public static Project Create(string name, string? description, Guid workspaceId)
    {
        return new Project(Guid.CreateVersion7(), name, description, workspaceId);
    }
}
