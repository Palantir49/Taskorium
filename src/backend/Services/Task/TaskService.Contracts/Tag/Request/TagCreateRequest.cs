namespace TaskService.Contracts.Tag.Request;

public record class TagCreateRequest(
    string name,
    Guid projectId);
