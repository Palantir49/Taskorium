namespace TaskService.Contracts.Tag;

public record class TagResponse(
    Guid id,
    string name,
    Guid projectId);
