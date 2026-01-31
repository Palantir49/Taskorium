namespace TaskService.Contracts.Type;

public record class TypeResponse(
    Guid id,
    string name,
    Guid projectId,
    string color);
