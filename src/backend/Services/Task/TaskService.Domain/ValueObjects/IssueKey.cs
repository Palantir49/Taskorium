using System.Text.RegularExpressions;

namespace TaskService.Domain.ValueObjects;

public record IssueKey
{
    public string Value { get; private set; }

    public IssueKey(string value)
    {
        value = value.Trim();
        IsValid(value);
        Value = value;
    }

    private void IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException("Ключ задачи задачи не может быть пустым или состоять только из пробелов",
            nameof(value));
        if(!Regex.IsMatch(value, "^[a-zA-Z]{1,5}-\\d{1,4}$"))
            throw new ArgumentNullException("Ключ задачи не соответствует маске ААААА-0000",
            nameof(value));
    }

    public override string ToString() => Value;
}
