using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class IssueStatus : BaseEntities
{
    public Guid ProjectId { get; }
    public IssueStatusType Type { get; private set; }
    public int Position { get; private set; }
    //FAQ: уточнить, будем ли что-то подобное хранить. Если да - сделать VO под HEX
    public string? Color { get; private set; }
    //FAQ: а статусу и типу вообще нужно CreateDate? а то мб и не нужно от BaseEntities наследоваться
    protected IssueStatus() { }

    private IssueStatus(Guid id, string name, IssueStatusType type, int position, string color, Guid projectId) : base(id, name)
    {
        Type = type;
        Position = position;
        Color = color;
        ProjectId = projectId;
    }

    public static IssueStatus Create(string name, string type, int position, string color, Guid projectId)
    {
        return new IssueStatus(
            id: Guid.CreateVersion7(),
            name: name,
            type: Enum.Parse<IssueStatusType>(type),
            position: position,
            color: color,
            projectId: projectId);
    }
    public void UpdateType(IssueStatusType type) => Type = type;

    public void UpdatePosition(int position) => Position = position;

    public void UpdateColor(string color) => Color = color;
}
