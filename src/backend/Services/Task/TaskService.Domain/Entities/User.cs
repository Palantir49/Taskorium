using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Guid KeycloakId { get; private set; }
    public EmailAdress Email { get; private set; }
    public UserName Username { get; private set; }
    public DateTimeOffset CreatedDate { get; }
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
