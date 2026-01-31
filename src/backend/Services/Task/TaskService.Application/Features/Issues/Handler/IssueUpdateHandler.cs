using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Issues.Handler
{
    public class IssueUpdateHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueUpdateCommand, IssueResponse>
    {
        public async Task<IssueResponse> Handle(IssueUpdateCommand request, CancellationToken cancellationToken = default)
        {
            Issue? issue = await wrapper.Issues.GetByIdAsync(request.id) ?? throw new NullReferenceException($"Задача с id: {request.id} не найдена");

            Project? project = await wrapper.Projects.GetByIdAsync(issue.ProjectId);

            IssueStatus? status = await wrapper.IssueStatus.GetByIdAsync(request.IssueStatusId) ??
                throw new NullReferenceException($"Статус задачи с id: {request.IssueStatusId} не найдена");

            //TODO: проверить что можно менять на этот статус

            IssueType? type = await wrapper.IssueType.GetByIdAsync(request.IssueTypeId) ??
                throw new NullReferenceException($"Тип задачи с id: {request.IssueTypeId} не найдена");

            //TODO: проверить что можно менять на этот тип

            issue.UpdateName(request.Name);
            issue.UpdateDescription(request.Description);
            //issue.UpdateType();
            //issue.UpdateStatus();
            issue.UpdateDueDate(request.DueDate);

            await wrapper.Issues.UpdateAsync(issue);
            await wrapper.SaveChangesAsync(cancellationToken);

            return issue.ToResponse();
        }
    }
}
