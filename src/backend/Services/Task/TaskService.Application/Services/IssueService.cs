using TaskService.Application.Interfaces;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;

namespace TaskService.Application.Services;

public class IssueService(IIssueRepository issueRepository) : IIssueService
{
    public async Task<IssueResponse> CreateTaskAsync(CreateIssueRequest request, CancellationToken ct = default)
    {
        //TODO: замечал, что указывается конкретно параметр:свойство. как правильно?
        var issue = Issue.Create(request.Name, request.Description, request.ProjectId, request.TaskTypeId,
            request.TaskStatusId, request.ReporterId, request.DueDate);
        await issueRepository.AddAsync(issue, ct);
        await issueRepository.SaveChangesAsync(ct);
        return new IssueResponse(issue.Id, issue.Name, issue.ProjectId, issue.TaskTypeId, issue.TaskStatusId,
            issue.CreatedDate, issue.Description, issue.ReporterId,
            issue.UpdatedDate, issue.DueDate, issue.ResolvedDate);
    }

    public async Task<IssueResponse?> GetTaskByIdAsync(Guid id, CancellationToken ct = default)
    {
        var issue = await issueRepository.GetByIdAsync(id, ct);
        //TODO: как правильно в этом случае?
        if (issue != null)
        {
            return ResponseMap(issue);
        }

        return null;
    }

    public async Task<List<IssueResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
    {
        var list = await issueRepository.GetByProjectIdAsync(projectId, ct);
        return list.Select(ResponseMap).ToList();
    }

    private static IssueResponse ResponseMap(Issue issue)
    {
        return new IssueResponse(issue.Id, issue.Name, issue.ProjectId, issue.TaskTypeId, issue.TaskStatusId,
            issue.CreatedDate, issue.Description, issue.ReporterId, issue.UpdatedDate, issue.DueDate,
            issue.ResolvedDate);
    }
}
