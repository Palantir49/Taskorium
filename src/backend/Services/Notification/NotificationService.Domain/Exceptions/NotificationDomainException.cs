namespace NotificationService.Domain.Exceptions;

public class NotificationDomainException : Exception
{
    public NotificationDomainException() { }

    public NotificationDomainException(string message)
        : base(message)
    {
    }
}
