namespace NotificationService.Domain.ValueObjects;

public record IdempotencyKey
{
    private IdempotencyKey(Guid value)
    {
        if (Guid.Empty == value)
        {
            throw new ArgumentException("IdempotencyKey cannot be empty");
        }

        Value = value;
    }

    public Guid Value { get; }

    public static IdempotencyKey FromEventId(Guid eventId)
    {
        return new IdempotencyKey(eventId);
    }

    public static IdempotencyKey Create(Guid value)
    {
        return new IdempotencyKey(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
