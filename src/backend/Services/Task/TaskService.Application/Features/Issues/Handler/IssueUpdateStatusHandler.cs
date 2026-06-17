using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler
{
    public class IssueUpdateStatusHandler(TaskServiceDbContext context, HybridCache cache) : IRequestHandler<IssueUpdateStatusCommand, IssueResponse>
    {
        public async Task<IssueResponse> Handle(IssueUpdateStatusCommand request, CancellationToken cancellationToken = default)
        {
            var issue = await context.Issues
                .Where(element => element.Id == request.Id)
                .Include(element => element.IssueAssignees)
                .ThenInclude(element => element.User)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NullReferenceException($"Задача с id: {request.Id} не найдена");

            _ = await context.Projects.FindAsync([issue.ProjectId], cancellationToken) ??
                throw new InvalidOperationException($"Проект с id: {issue.ProjectId}, связанный с задачей, не существует");
            var status = await context.IssueStatus.FindAsync([request.NewStatusId], cancellationToken) ??
                         throw new NullReferenceException($"Статус задачи с id: {request.NewStatusId} не найден");

            issue.UpdateStatus(status);

            context.Issues.Update(issue);
            await context.SaveChangesAsync(cancellationToken);
            //Инвалидируем кэш:
            // 1. Задача
            var issueCacheKey = $"issue_id_{issue.Id}";
            await cache.RemoveAsync(issueCacheKey, cancellationToken);
            // 2. Список задач проекта
            var projectIssuesCacheKey = $"issues_by_project_id_{issue.ProjectId}";
            await cache.RemoveAsync(projectIssuesCacheKey, cancellationToken);

            return issue.ToResponse();
        }
    }
}
