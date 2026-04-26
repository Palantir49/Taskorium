using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Tag : BaseEntities, ISoftDeletable
{
    public Guid ProjectId { get; }

    public List<Issue> Issues { get; private set; } = new();
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    protected Tag() { }

    private Tag(Guid id, string name, Guid projectId) : base(id, name)
    {
        ProjectId = projectId;
    }

    public static Tag Create(string name, Guid projectId)
    {
        return new Tag(
            id: Guid.CreateVersion7(),
            name: name,
            projectId:
            projectId);
    }
}
