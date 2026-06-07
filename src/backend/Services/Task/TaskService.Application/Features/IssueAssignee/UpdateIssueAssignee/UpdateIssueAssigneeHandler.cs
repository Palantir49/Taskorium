using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.UpdateIssueAssignee;

public class UpdateIssueAssigneeHandler(TaskServiceDbContext context)
    : IRequestHandler<UpdateIssueAssigneeCommand, IssueAssigneesResponce>
{
    public async Task<IssueAssigneesResponce> Handle(UpdateIssueAssigneeCommand request,
        CancellationToken cancellationToken = default)
    {
        if (request.Role == AssigneesRoles.Creator)
        {
            throw new InvalidOperationException("Нельзя назначить создателя");
        }

        var assignees =
            await context.IssueAssignees.FirstOrDefaultAsync(
                x => x.IssueId == request.IssueId && x.UserId == request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Ответственный не найден");

        if (assignees.Role == AssigneesRoles.Creator)
        {
            throw new InvalidOperationException("Нельзя изменить создателя задачи");
        }

        assignees.UpdateRole(request.Role);
        await context.SaveChangesAsync(cancellationToken);

        return new IssueAssigneesResponce(
            assignees.IssueId,
            assignees.UserId,
            assignees.Role.ToDto());
    }
}
