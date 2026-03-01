using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Tags.Handler;

public class TagCreateHandler(TaskServiceDbContext context) : IRequestHandler<TagCreateCommand, TagResponse>
{
    public async Task<TagResponse> Handle(TagCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.projectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");

        Tag tag = Tag.Create(
            name: request.name,
            projectId: request.projectId);

        await context.Tag.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
