using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.DeleteIssueAssignee;

public class DeleteIssueAssigneeHandler(TaskServiceDbContext context) : IRequestHandler<DeleteIssueAssigneeCommand, int>
{
    public async Task<int> Handle(DeleteIssueAssigneeCommand request, CancellationToken cancellationToken = default)
    {
        var assignee =
            await context.IssueAssignees.FirstOrDefaultAsync(x =>
                x.IssueId == request.IssueId && x.UserId == request.UserId)
            ?? throw new KeyNotFoundException("Ответственный не найден");

        if (assignee.AssigneesRoles == AssigneesRoles.Creator)
        {
            throw new InvalidOperationException("Нельзя удалить создателя задачи");
        }

        context.Remove(assignee);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
