using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Workspace : BaseEntities, ISoftDeletable
{
    protected Workspace() { }

    private Workspace(Guid id, string name) : base(id, name)
    {
        //OwnerId = ownerId;
    }
    //public Guid? OwnerId { get; private set; }
    public List<WorkspaceMember> WorkspaceMembers { get; private set; } = [];
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public static Workspace Create(string name)
    {
        return new Workspace(Guid.CreateVersion7(), name);
    }
}
