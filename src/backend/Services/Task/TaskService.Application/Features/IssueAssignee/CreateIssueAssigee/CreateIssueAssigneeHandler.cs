using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public class CreateIssueAssigneeHandler(TaskServiceDbContext context) : IRequestHandler<CreateIssueAssigneeCommand, IssueAssigneesResponce>
{
    public async Task<IssueAssigneesResponce> Handle(CreateIssueAssigneeCommand request, CancellationToken cancellationToken = default)
    {
        if (request.Role == (int)AssigneesRoles.Creator)
            throw new InvalidOperationException("Нельзя назначить создателя");

        User user = await context.Users.FindAsync([request.UserId], cancellationToken) 
            ?? throw new KeyNotFoundException($"Пользователь не найден");

        Issue issue = await context.Issues.FindAsync([request.IssueId], cancellationToken)
            ?? throw new KeyNotFoundException($"Задача не найдена");

        IssueAssignees assignees = IssueAssignees.Create(
            userId: request.UserId,
            issueId: request.IssueId,
            role: request.Role);

        context.Add(assignees);
        await context.SaveChangesAsync();

        return new IssueAssigneesResponce(
            IssueId: issue.Id,
            UserId: user.Id,
            Role: (int)assignees.Role);
    }
}
