using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities;

public class User
{
    private User()
    {
    }

    private User(Guid id, Guid keycloakId, UserName userName, EmailAdress email, DateTimeOffset createdAt,
        string fullName)
    {
        Id = id;
        Username = userName;
        KeycloakId = keycloakId;
        Email = email;
        CreatedDate = createdAt;
        FullName = fullName;
    }

    public Guid Id { get; }
    public Guid KeycloakId { get; private set; }
    public EmailAdress Email { get; private set; } = null!;
    public UserName Username { get; private set; } = null!;
    public DateTimeOffset CreatedDate { get; }
    public string FullName { get; } = null!;
    public List<WorkspaceMember> WorkspaceMembers { get; private set; } = [];
    public List<ProjectMember> ProjectMembers { get; private set; } = [];


    public static User Create(Guid keycloakId, UserName userName, EmailAdress email, string fullName)
    {
        return new User(Guid.CreateVersion7(), keycloakId, userName, email, DateTimeOffset.UtcNow, fullName);
    }

    public void UpdateEmail(EmailAdress newEmail)
    {
        Email = newEmail;
    }
}
