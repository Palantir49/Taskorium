using TaskService.Domain.Entities.BaseEntity;

namespace TaskService.Domain.Entities;

public class Project : BaseEntityTask
{
    //TODO:будем ли тут фиксировать дату изменения
    //Если да, являются ли изменения дочерних объектов тоже изменением проекта?

    //TODO: дату когда закончилось и началось
    private Project(Guid id, string name, string? description, Guid workspaceId, DateTimeOffset createdDate)
        : base(id, name, createdDate)
    {
        WorkspaceId = workspaceId;
        Description = description;
    }

    public Guid WorkspaceId { get; }

    public string? Description { get; private set; }

    public static Project Create(string name, string? description, Guid workspaceId)
    {
        return new Project(Guid.CreateVersion7(), name, description, workspaceId, DateTimeOffset.UtcNow);
    }
}
