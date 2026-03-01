namespace TaskService.Contracts.Tag.Request;

public record class TagUpdateRequest(
    Guid id,
    string name,
    string? color);
