using Microsoft.EntityFrameworkCore;
using TaskService.Application.Exceptions;
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

        bool hasIssues = await context.Set<Issue>()
            .AnyAsync(x => x.IssueStatusId == status.Id, cancellationToken);

        if (hasIssues)
            throw new ConflictException($"Нельзя удалить статус, пока существуют связанные задачи");
        //TODO: изменить исключение на подходящее

        var removeres = context.IssueStatus.Remove(status);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
