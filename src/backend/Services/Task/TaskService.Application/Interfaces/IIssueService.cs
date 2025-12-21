using TaskService.Contracts.Task.Requests;
using TaskService.Contracts.Task.Responses;

namespace TaskService.Application.Interfaces
{
    public interface IIssueService
    {
        //TODO: уточнить, как должны происходить изменения на этом этапе - с использованием DTO или нет?
        //Да гуд
        Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct = default);
        Task<TaskResponse?> GetTaskByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<TaskResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    }
}
