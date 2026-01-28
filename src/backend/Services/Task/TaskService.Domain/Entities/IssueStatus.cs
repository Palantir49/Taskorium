using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class IssueStatus : BaseEntities
{
    public IssueStatusType Type { get; private set; }
    public int Position { get; private set; }
    //TODO: уточнить, будем ли что-то подобное хранить. Если да - сделать VO под HEX
    public string Color { get; private set; }

    protected IssueStatus()
    {
        Color = "#0000FF";
    }

    private IssueStatus(Guid id, string name, IssueStatusType type, int position, string color) : base(id, name)
    {
        Type = type;
        Position = position;
        Color = color;
    }

    public static IssueStatus Create(Guid id, string name, string type, int position, string color)
    {
        return new IssueStatus(id: id, name: name, type: Enum.Parse<IssueStatusType>(type), position: position, color: color);
    }

    public void UpdatePosition(int position) => Position = position;

    public void UpdateColor(string color) => Color = color;
}
