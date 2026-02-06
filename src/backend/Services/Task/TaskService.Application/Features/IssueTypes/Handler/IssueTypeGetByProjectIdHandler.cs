using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeGetByProjectIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueTypeGetByProjectIdQuery, IEnumerable<IssueTypeResponse>>
{
    public async Task<IEnumerable<IssueTypeResponse>> Handle(IssueTypeGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<IssueType> types = await context.IssueType
            .Where(x => x.ProjectId == request.id)
            .ToListAsync(cancellationToken);

        return types.Select(x => x.ToResponse());
    }
}
