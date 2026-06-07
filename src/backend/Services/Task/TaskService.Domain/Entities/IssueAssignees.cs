using TaskService.Domain.Entities.Enums;

namespace TaskService.Domain.Entities;

public class IssueAssignees
{
    protected IssueAssignees() { }

    private IssueAssignees(Guid userId, Guid issueId, AssigneesRoles role)
    {
        UserId = userId;
        IssueId = issueId;
        Role = role;
    }


    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid IssueId { get; private set; }
    public Issue Issue { get; private set; } = null!;


    public AssigneesRoles Role { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }


    public static IssueAssignees Create(Guid userId, Guid issueId, AssigneesRoles role)
    {
        return new IssueAssignees(
            userId,
            issueId,
            role);
    }

    public static IssueAssignees Create(Guid userId, Guid issueId, int role)
    {
        return new IssueAssignees(
            userId,
            issueId,
            (AssigneesRoles)role);
    }


    public void SetUser(User user)
    {
        User = user;
    }


    public void UpdateRole(AssigneesRoles role)
    {
        Role = role;
    }

    public void UpdateRole(int role)
    {
        Role = (AssigneesRoles)role;
    }
}
