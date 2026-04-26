namespace NotificationService.Domain.ValueObjects;

public record RecipientId
{
    private RecipientId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("RecipientId cannot be empty");
        }

        Value = value;
    }

    public string Value { get; }

    public static RecipientId Create(string value)
    {
        return new RecipientId(value);
    }

    public static RecipientId FromUser(string userId)
    {
        return new RecipientId(userId);
    }

    public override string ToString()
    {
        return Value;
    }
}
