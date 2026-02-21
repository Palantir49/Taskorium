using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.IssueTags.Handler;

public class IssueTagGetByProjectIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueTagGetByProjectIdQuery, IEnumerable<IssueTagResponse>>
{
    public async Task<IEnumerable<IssueTagResponse>> Handle(IssueTagGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<IssueTag> tags = await context.IssueTag
            .Where(x => x.ProjectId == request.id)
            .ToListAsync(cancellationToken);

        return tags.Select(x => x.ToResponse());
    }
}
