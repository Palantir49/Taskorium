using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Issues.Handler;

internal class IssueDeleteByIdhandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueDeleteByIdCommand, int>
{
    public async Task<int> Handle(IssueDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        Issue issue = await wrapper.Issues.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Задача с id: {request.id} не найдена");

        await wrapper.Issues.DeleteAsync(issue, cancellationToken);
        return await wrapper.SaveChangesAsync(cancellationToken);
    }
}
