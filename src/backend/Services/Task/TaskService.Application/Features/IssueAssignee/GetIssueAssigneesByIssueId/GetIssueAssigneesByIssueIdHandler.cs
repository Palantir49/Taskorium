using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.GetIssueAssigneesByIssueId
{
    internal class GetIssueAssigneesByIssueIdHandler(TaskServiceDbContext context) :
        IRequestHandler<GetIssueAssigneesByIssueIdQuery, IEnumerable<IssueAssigneesResponce>>
    {
        public async Task<IEnumerable<IssueAssigneesResponce>> Handle(GetIssueAssigneesByIssueIdQuery request, CancellationToken cancellationToken = default)
        {
            Issue issue = await context.Issues.Include(x => x.IssueAssignees).FirstOrDefaultAsync(x => x.Id == request.IssueId)
                ?? throw new KeyNotFoundException($"Задача не найдена");

            return issue.IssueAssignees.Select(x => new IssueAssigneesResponce(
                IssueId: x.IssueId,
                UserId: x.UserId,
                Role: (int)x.Role));
        }
    }
}
