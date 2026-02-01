using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusDeleteByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueStatusDeleteByIdCommand, int>
{
    public async Task<int> Handle(IssueStatusDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        IssueStatus status = await wrapper.IssueStatus.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Статус с id: {request.id} не найден");

        await wrapper.IssueStatus.DeleteAsync(status, cancellationToken);
        return await wrapper.SaveChangesAsync(cancellationToken);
    }
}
