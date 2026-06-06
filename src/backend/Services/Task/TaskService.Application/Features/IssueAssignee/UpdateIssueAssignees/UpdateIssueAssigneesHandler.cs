using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using TaskService.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace TaskService.Application.Features.IssueAssignee.UpdateIssueAssignees
{
    public class UpdateIssueAssigneesHandler(TaskServiceDbContext context) : IRequestHandler<UpdateIssueAssigneesCommand, IssueAssigneesResponce>
    {
        public async Task<IssueAssigneesResponce> Handle(UpdateIssueAssigneesCommand request, CancellationToken cancellationToken = default)
        {
            IssueAssignees assignees = await context.IssueAssignees.FirstOrDefaultAsync(x => x.IssueId == request.IssueId && x.UserId == request.UserId, cancellationToken)
                ?? throw new KeyNotFoundException($"Ответственный не найден");

            if (assignees.Role == AssigneesRoles.Creator)
                throw new InvalidOperationException("Нельзя изменить создателя задачи");

            assignees.UpdateRole(request.Role);
            await context.SaveChangesAsync(cancellationToken);

            return new IssueAssigneesResponce(
                IssueId: assignees.IssueId,
                UserId: assignees.UserId,
                Role: (int)assignees.Role);
        }
    }
}
