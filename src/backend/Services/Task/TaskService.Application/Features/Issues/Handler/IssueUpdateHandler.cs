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
            Issue? issue = await wrapper.Issues.GetByIdAsync(request.id);

            if (issue == null)
                throw new NullReferenceException($"задача с id: {request.id} не найдена");

            Project? project = await wrapper.Projects.GetByIdAsync(issue.ProjectId);

            if (project == null)

            //TODO: проверка существования статуса
            //TODO: проверка существования типа

            issue.UpdateName(request.Name);
            issue.UpdateDescription(request.Description);
            //issue.UpdateType();
            //issue.UpdateStatus();
            issue.UpdateDueDate(request.DueDate);
            
            await wrapper.SaveChangesAsync(cancellationToken);

            return issue.ToResponce();
        }
    }
}
