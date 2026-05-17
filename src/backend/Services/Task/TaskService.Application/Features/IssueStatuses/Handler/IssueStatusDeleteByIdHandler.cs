using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusDeleteByIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueStatusDeleteByIdCommand, int>
{
    public async Task<int> Handle(IssueStatusDeleteByIdCommand request, CancellationToken cancellationToken = default)
    {
        IssueStatus status = await context.IssueStatus.FindAsync(new object[] { request.id }) ??
            throw new NullReferenceException($"Статус с id: {request.id} не найден");

        //FAQ: а точно в репозитории для этого нужно заводить или через all делать?
        List<Issue> issue = await context.Set<Issue>().ToListAsync();
        //GetByIssueStatusIdAsync(statusId: status.Id, cancellationToken);

        if (issue != null && issue.Count > 0)
            throw new NullReferenceException($"Нельзя удалить статус, пока существуют связанные задачи");
        //TODO: изменить исключение на подходящее

        var removeres = context.IssueStatus.Remove(status);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
