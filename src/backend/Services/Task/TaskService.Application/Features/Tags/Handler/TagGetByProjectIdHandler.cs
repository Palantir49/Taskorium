using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.Tags.Handler;

public class TagGetByProjectIdHandler(TaskServiceDbContext context) : IRequestHandler<TagGetByProjectIdQuery, IEnumerable<TagResponse>>
{
    public async Task<IEnumerable<TagResponse>> Handle(TagGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<Tag> tags = await context.Tags
            .Where(x => x.ProjectId == request.id)
            .ToListAsync(cancellationToken);

        return tags.Select(x => x.ToResponse());
    }
}
