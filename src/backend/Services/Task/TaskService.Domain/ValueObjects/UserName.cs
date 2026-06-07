namespace TaskService.Domain.ValueObjects;

public class UserName
{
    public UserName(string value)
    {
        var trimValue = value.Trim();
        IsValidName(trimValue);
        Value = value;
    }

    public string Value { get; private set; }

    private static void IsValidName(string? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Имя пользователя не может быть null");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException("Имя пользователя не может быть пустым или состоять только из пробелов",
                nameof(value));
        }
    }

    public override string ToString()
    {
        return Value;
    }
}
