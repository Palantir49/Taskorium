using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Guid KeycloakId { get; private set; }
    public EmailAdress Email { get; private set; }
    public UserName Username { get; private set; }
    public DateTimeOffset CreatedDate { get; }
    //TODO: Это нужно убрать, обращение идет через репозитории
    public ICollection<Workspace> Workspaces { get; private set; } = new List<Workspace>();
    public ICollection<Issue> Issues { get; private set; } = new List<Issue>();
    protected User()
    {
        Email = null!;
        Username = null!;
    }
    private User(Guid id, Guid keycloakId, UserName userName, EmailAdress email)
    {
        Id = id;
        Username = userName;
        KeycloakId = keycloakId;
        Email = email;
    }
    public static User Create(Guid keycloakId, UserName userName, EmailAdress email)
    {
        return new User(Guid.CreateVersion7(), keycloakId, userName, email);
    }
    public void UpdateEmail(EmailAdress newEmail)
    {
        Email = newEmail;
    }
}
