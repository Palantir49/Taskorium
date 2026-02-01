using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueStatuses.Handler;

internal class IssueStatusUpdateHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueStatusUpdateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusUpdateCommand request, CancellationToken cancellationToken = default)
    {
        IssueStatus status = await wrapper.IssueStatus.GetByIdAsync(request.id, cancellationToken) ?? 
            throw new NullReferenceException($"Статус с id: {request.id} не найден");

        status.UpdateName(request.name);
        status.UpdatePosition(request.position);
        status.UpdateType(Enum.Parse<IssueStatusType>(request.type));
        status.UpdateColor(request.color);

        await wrapper.IssueStatus.UpdateAsync(status, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
