using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusUpdateHandler(TaskServiceDbContext context) : IRequestHandler<IssueStatusUpdateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusUpdateCommand request, CancellationToken cancellationToken = default)
    {
        IssueStatus status = await context.IssueStatus.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Статус с id: {request.id} не найден");

        status.UpdateName(request.name);
        status.UpdatePosition(request.position);
        status.UpdateType(Enum.Parse<IssueStatusType>(request.type));
        status.UpdateColor(request.color);

        context.IssueStatus.Update(status);
        await context.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
