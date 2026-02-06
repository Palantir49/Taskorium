using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueDeleteByIdhandler(TaskServiceDbContext context) : IRequestHandler<IssueDeleteByIdCommand, int>
{
    public async Task<int> Handle(IssueDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        Issue issue = await context.Issues.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Задача с id: {request.id} не найдена");

        context.Issues.Remove(issue);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
