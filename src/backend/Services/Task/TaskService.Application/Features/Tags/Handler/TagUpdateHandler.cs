using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Tags.Handler;

public class TagUpdateHandler(TaskServiceDbContext context) : IRequestHandler<TagUpdateCommand, TagResponse>
{
    public async Task<TagResponse> Handle(TagUpdateCommand request, CancellationToken cancellationToken = default)
    {
        Tag tag = await context.Tag.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

        tag.UpdateName(request.name);

        context.Tag.Update(tag);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
