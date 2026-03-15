using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Tags.Handler
{
    public class TagGetByIdhandler(TaskServiceDbContext context) : IRequestHandler<TagGetByIdQuery, TagResponse>
    {
        public async Task<TagResponse> Handle(TagGetByIdQuery request, CancellationToken cancellationToken = default)
        {
            Tag tag = await context.Tags.FindAsync(request.id, cancellationToken) ??
                throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");
            return tag.ToResponse();
        }
    }
}
