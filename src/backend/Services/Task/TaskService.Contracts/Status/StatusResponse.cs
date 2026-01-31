
namespace TaskService.Contracts.Status;

public record class StatusResponse(
    Guid id,
    string name,
    Guid projectId,
    string type,
    int position,
    string color);
