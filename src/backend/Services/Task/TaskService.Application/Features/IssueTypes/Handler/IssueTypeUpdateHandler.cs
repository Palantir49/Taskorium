using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeUpdateHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueTypeUpdateCommand, IssueTypeResponse>
{
    public async Task<IssueTypeResponse> Handle(IssueTypeUpdateCommand request, CancellationToken cancellationToken = default)
    {
        IssueType type = await wrapper.IssueType.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

        type.UpdateName(request.name);
        type.UpdateColor(request.color);

        await wrapper.IssueType.UpdateAsync(type, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);
        return type.ToResponse();
    }
}
