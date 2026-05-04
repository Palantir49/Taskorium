using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class ProjectMember : ISoftDeletable
{
    protected ProjectMember()
    {
    }

    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }
    public ProjectRoles Role { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;

    private ProjectMember(Guid projectId, Guid userId, ProjectRoles role, DateTimeOffset joinedAt)
    {
        UserId = userId;
        ProjectId = projectId;
        Role = role;
        JoinedAt = joinedAt;
    }
    public static ProjectMember Create(Guid projectId, Guid userId, ProjectRoles role, DateTimeOffset joinedAt)
    {
        return new ProjectMember(projectId, userId, role, joinedAt);
    }
}
