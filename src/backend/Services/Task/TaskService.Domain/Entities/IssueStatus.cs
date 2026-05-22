using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class IssueStatus : BaseEntities, ISoftDeletable
{
    public Guid ProjectId { get; }
    public IssueStatusType Type { get; private set; }
    public int Position { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    //FAQ: а статусу и типу вообще нужно CreateDate? а то мб и не нужно от BaseEntities наследоваться
    protected IssueStatus() { }

    private IssueStatus(Guid id, string name, IssueStatusType type, int position, Guid projectId) : base(id, name)
    {
        Type = type;
        Position = position;
        ProjectId = projectId;
    }

    public static IssueStatus Create(string name, int numberType, int position, Guid projectId)
    {
        return new IssueStatus(
            id: Guid.CreateVersion7(),
            name: name,
            type: (IssueStatusType)numberType,
            position: position,
            projectId: projectId);
    }
    public void UpdateType(IssueStatusType type) => Type = type;

    public void UpdatePosition(int position) => Position = position;
}
