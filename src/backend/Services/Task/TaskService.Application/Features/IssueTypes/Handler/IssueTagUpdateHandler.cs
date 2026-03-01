using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTags.Handler;

public class IssueTagUpdateHandler(TaskServiceDbContext context) : IRequestHandler<IssueTagUpdateCommand, IssueTagResponse>
{
    public async Task<IssueTagResponse> Handle(IssueTagUpdateCommand request, CancellationToken cancellationToken = default)
    {
        IssueTag tag = await context.IssueTag.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

        tag.UpdateName(request.name);

        context.IssueTag.Update(tag);
        await context.SaveChangesAsync(cancellationToken);
        return tag.ToResponse();
    }
}
