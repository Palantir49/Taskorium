using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusGetByProjectIdHandler(TaskServiceDbContext context)
    : IRequestHandler<IssueStatusGetByProjectIdQuery, IEnumerable<IssueStatusResponse>>
{
    public async Task<IEnumerable<IssueStatusResponse>> Handle(IssueStatusGetByProjectIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var statuses = await context.IssueStatus
            .Where(x => x.ProjectId == request.id)
            .ToListAsync(cancellationToken);

        return statuses.Select(x => x.ToResponse());
    }
}
