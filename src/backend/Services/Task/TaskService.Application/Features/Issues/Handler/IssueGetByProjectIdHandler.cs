using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetByProjectIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueGetByProjectIdQuery, IEnumerable<IssueResponse>>
{
    public async Task<IEnumerable<IssueResponse>> Handle(IssueGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<Issue> issues = await wrapper.Issues.GetByProjectIdAsync(request.projectId, cancellationToken);

        return issues.Select(x => x.ToResponce());
    }
}
