using TaskService.Domain.Entities.BaseEntity;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class IssueAssignees : ISoftDeletable
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid IssueId { get; private set; }
    public Issue Issue { get; private set; } = null!;

    public Roles Role { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    protected IssueAssignees() { }

    private IssueAssignees(Guid userId, Guid issueId, Roles role)
    {
        UserId = userId;
        IssueId = issueId;
        Role = role;
    }

    public static IssueAssignees Create(Guid userId, Guid issueId, Roles role)
    {
        return new IssueAssignees(
            userId: userId,
            issueId: issueId,
            role: role);
    }

    public void UpdateRole(Roles role) => Role = role;
}
