using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler
{
    public class IssueUpdateHandler(TaskServiceDbContext context) : IRequestHandler<IssueUpdateCommand, IssueResponse>
    {
        public async Task<IssueResponse> Handle(IssueUpdateCommand request, CancellationToken cancellationToken = default)
        {
            Issue? issue = await context.Issues.FindAsync(request.id, cancellationToken) ??
                throw new NullReferenceException($"Задача с id: {request.id} не найдена");

            Project? project = await context.Projects.FindAsync(issue.ProjectId, cancellationToken);
            //TODO: добавить исключение, которое будет именно серверное, т.к. в этом случае у задачи не существующий проект
            //FAQ: какое исключение будет серверным в этом случае?

            IssueStatus? status = await context.IssueStatus.FindAsync(request.IssueStatusId, cancellationToken) ??
                throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найдена");

            //TODO: проверить что можно менять на этот статус

            IssueTag? tag = await context.IssueTag.FindAsync(request.IssueTagId, cancellationToken) ??
                throw new NullReferenceException($"Тип задачи с id: {request.IssueTagId} не найдена");

            //TODO: проверить что можно менять на этот тип

            issue.UpdateName(request.Name);
            issue.UpdateDescription(request.Description);
            issue.UpdateTag(tag.Id);
            issue.UpdateStatus(status);
            issue.UpdateDueDate(request.DueDate);

            context.Issues.Update(issue);
            await context.SaveChangesAsync(cancellationToken);

            return issue.ToResponse();
        }
    }
}
