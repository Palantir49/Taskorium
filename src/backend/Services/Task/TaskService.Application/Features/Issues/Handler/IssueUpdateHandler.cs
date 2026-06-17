using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueUpdateHandler(
    TaskServiceDbContext context,
    HybridCache cache,
    IValidator<IssueUpdateCommand> validator)
    : IRequestHandler<IssueUpdateCommand, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueUpdateCommand request, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        var issue = await context.Issues.Where(element => element.Id == request.Id)
                        .Include(element => element.IssueAssignees)
                            .ThenInclude(element => element.User)
                        .FirstOrDefaultAsync(cancellationToken) ??
                    throw new NullReferenceException($"Задача с id: {request.Id} не найдена");

        _ = await context.Projects.FindAsync([issue.ProjectId], cancellationToken) ??
            throw new InvalidOperationException($"Проект с id: {issue.ProjectId}, связанный с задачей, не существует");
        var status = await context.IssueStatus.FindAsync([request.IssueStatusId], cancellationToken) ??
                     throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найден");


        issue.UpdateName(request.Name);
        issue.UpdateDescription(request.Description);
        issue.UpdateType(request.NumberIssueType);
        issue.UpdateStatus(status);
        issue.UpdateDueDate(request.DueDate);
        UpdateAssignees(request, issue);

        context.Issues.Update(issue);
        await context.SaveChangesAsync(cancellationToken);
        //Инвалидируем кэш:
        var issueCacheKey = $"issue_id_{issue.Id}";
        var issueCacheKeyV2 = $"issue_id_v2_{issue.Id}";
        // 2. Список задач проекта
        var projectIssuesCacheKey = $"issues_by_project_id_{issue.ProjectId}";
        var projectIssuesCacheKeyV2 = $"issues_by_project_id_v2_{issue.ProjectId}";
        await cache.RemoveAsync(issueCacheKey, cancellationToken);
        await cache.RemoveAsync(issueCacheKeyV2, cancellationToken);
        await cache.RemoveAsync(projectIssuesCacheKey, cancellationToken);
        await cache.RemoveAsync(projectIssuesCacheKeyV2, cancellationToken);
        return issue.ToResponse();
    }


    private void UpdateAssignees(IssueUpdateCommand request, Issue issue)
    {
        if (request.Assignees is not { Count: > 0 })
        {
            return;
        }

        var requestedUserIds = request.Assignees.Select(d => d.UserId).ToHashSet();
        var existingAssignees = issue.IssueAssignees.ToList();
        var existingUserIds = existingAssignees.Select(a => a.UserId).ToHashSet();

        if (requestedUserIds.SetEquals(existingUserIds))
        {
            var users = context.Users
                .Where(u => requestedUserIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            issue.IssueAssignees.ToList().ForEach(dto =>
            {
                if (users.TryGetValue(dto.UserId, out var user))
                {
                    dto.SetUser(user);
                }
            });
        }


        // Добавляем только тех, кого ещё нет
        var toAdd = request.Assignees
            .Where(dto => !existingUserIds.Contains(dto.UserId))
            .ToList();

        if (toAdd.Count > 0)
        {
            var users = context.Users
                .Where(u => toAdd.Select(d => d.UserId).Contains(u.Id))
                .ToDictionary(u => u.Id);

            var newAssignees = toAdd.Select(dto =>
                {
                    var assignee = IssueAssignees.Create(dto.UserId, request.Id, dto.ProjectRolesDto.ToEntity());
                    if (users.TryGetValue(dto.UserId, out var user))
                    {
                        assignee.SetUser(user);
                    }

                    return assignee;
                })
                .ToList()
                .AsReadOnly();

            issue.AddAssignees(newAssignees);
        }

        // Удаляем тех, кого нет в новом списке
        var assigneesToDelete = existingAssignees
            .Where(a => !requestedUserIds.Contains(a.UserId))
            .ToList()
            .AsReadOnly();

        if (assigneesToDelete.Count > 0)
        {
            issue.DeleteAssignees(assigneesToDelete);
        }
    }
}
