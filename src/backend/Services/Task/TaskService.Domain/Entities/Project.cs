using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Project : BaseEntities, ISoftDeletable
{
    protected Project() { }

    //TODO: добавить сокращение для key (5 символов)
    private Project(Guid id, string name, string? description, string abbreviation, Guid workspaceId)
        : base(id, name)
    {
        WorkspaceId = workspaceId;
        Description = description;
        Abbreviation = abbreviation;
    }

    public Guid WorkspaceId { get; }

    public string? Description { get; private set; }

    public DateTimeOffset? StartDate { get; private set; }
    public string Abbreviation { get; private set; } = null!;
    public DateTimeOffset? FinishDate { get; private set; }

    public List<IssueStatus>? Statuses { get; protected set; } = new();
    public List<ProjectMember> ProjectMembers { get; private set; } = [];
    public Workspace Workspace { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public static Project Create(string name, string? description, string abbreviation, Guid workspaceId)
    {
        return new Project(
            Guid.CreateVersion7(),
            name,
            description,
            abbreviation,
            workspaceId);
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
}
