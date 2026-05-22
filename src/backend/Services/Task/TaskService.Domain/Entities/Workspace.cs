using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Workspace : BaseEntities, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public ICollection<WorkspaceMember> WorkspaceMembers { get; private set; } = [];
    public ICollection<Project> Projects { get; private set; } = [];
    protected Workspace() { }
    private Workspace(Guid id, string name) : base(id, name) { }
    public static Workspace Create(string name)
    {
        return new Workspace(Guid.CreateVersion7(), name);
    }
}
