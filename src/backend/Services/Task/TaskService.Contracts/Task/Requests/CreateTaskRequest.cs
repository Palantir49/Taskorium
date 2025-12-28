
namespace TaskService.Contracts.Task.Requests
{
    public record class CreateTaskRequest(
        string Name, Guid ProjectId, Guid TaskTypeId, Guid TaskStatusId,
        string? Description = null, Guid? ReporterId = null, DateTimeOffset? DueDate = null);
}
