using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetByProjectIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueGetByProjectIdQuery, IEnumerable<IssueResponse>>
{
    public async Task<IEnumerable<IssueResponse>> Handle(IssueGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<Issue> issues = await context.Issues
            .Where(x => x.ProjectId == request.projectId)
            .ToListAsync(cancellationToken);

        return issues.Select(x => x.ToResponse());
    }
}
