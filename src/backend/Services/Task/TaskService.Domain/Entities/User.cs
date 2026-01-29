using TaskService.Domain.ValueObjects;

namespace TaskService.Domain.Entities;

public class User
{
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
    public EmailAdress Email { get; private set; }
    public UserName Username { get; private set; }
    public DateTimeOffset CreatedDate { get; }

    public string FullName { get; }

    public static User Create(Guid keycloakId, UserName userName, EmailAdress email, string fullName)
    {
        return new User(Guid.CreateVersion7(), keycloakId, userName, email, DateTimeOffset.Now, fullName);
    }

    public void UpdateEmail(EmailAdress newEmail)
    {
        Email = newEmail;
    }
}
