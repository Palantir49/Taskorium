using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetAllHandler(TaskServiceDbContext context) : IRequestHandler<GetAllIssuesQuery, IssuesResponse>
{
    public async Task<IssuesResponse> Handle(GetAllIssuesQuery request, CancellationToken cancellationToken = default)
    {
        var issues = await context.Issues
            .ToListAsync(cancellationToken);

        return new IssuesResponse(issues.Select(x => x.ToResponse()).ToList());
    }
}
