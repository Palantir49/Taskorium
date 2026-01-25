namespace TaskService.Domain.Entities;

public class ProjectMember
{
    protected ProjectMember()
    {
    }

    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }
    public string Role { get; private set; } = null!;
    public DateTimeOffset JoinedAt { get; private set; }
}
