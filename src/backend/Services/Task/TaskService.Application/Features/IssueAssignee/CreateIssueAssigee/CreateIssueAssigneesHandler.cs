using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public class CreateIssueAssigneesHandler(TaskServiceDbContext context) : IRequestHandler<CreateIssueAssigneesCommand, IssueAssigneesResponce>
{
    public async Task<IssueAssigneesResponce> Handle(CreateIssueAssigneesCommand request, CancellationToken cancellationToken = default)
    {
        User user = await context.Users.FindAsync([request.UserId], cancellationToken) 
            ?? throw new KeyNotFoundException($"Пользователь не найден");

        Issue issue = await context.Issues.FindAsync([request.IssueId], cancellationToken)
            ?? throw new KeyNotFoundException($"Задача не найдена");

        IssueAssignees assignees = IssueAssignees.Create(
            userId: request.UserId,
            issueId: request.IssueId,
            role: request.role);

        context.Add(assignees);
        await context.SaveChangesAsync();

        return new IssueAssigneesResponce(
            IssueId: issue.Id,
            UserId: user.Id,
            Role: (int)assignees.Role);
    }
}
