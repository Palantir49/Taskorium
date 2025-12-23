using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Interfaces;

public interface IIssueService
{
    //TODO: уточнить, как должны происходить изменения на этом этапе - с использованием DTO или нет?
    //Да гуд
    Task<IssueResponse> CreateTaskAsync(CreateIssueRequest request, CancellationToken ct = default);
    Task<IssueResponse?> GetTaskByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<IssueResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
}
