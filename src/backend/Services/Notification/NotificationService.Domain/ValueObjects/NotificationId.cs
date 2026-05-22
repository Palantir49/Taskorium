namespace NotificationService.Domain.ValueObjects;

public record NotificationId
{
    private NotificationId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("NotificationId cannot be empty");
        }

        Value = value;
    }

    public Guid Value { get; }

    public static NotificationId CreateNew()
    {
        return new NotificationId(Guid.CreateVersion7());
    }

    public static NotificationId From(Guid value)
    {
        return new NotificationId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
