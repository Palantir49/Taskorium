using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeUpdateHandler(TaskServiceDbContext context) : IRequestHandler<IssueTypeUpdateCommand, IssueTypeResponse>
{
    public async Task<IssueTypeResponse> Handle(IssueTypeUpdateCommand request, CancellationToken cancellationToken = default)
    {
        IssueType type = await context.IssueType.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

        type.UpdateName(request.name);
        type.UpdateColor(request.color);

        context.IssueType.Update(type);
        await context.SaveChangesAsync(cancellationToken);
        return type.ToResponse();
    }
}
