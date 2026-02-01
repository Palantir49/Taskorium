using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Issues.Handler;

public class IssueGetByIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueGetByIdQuery, IssueResponse>
{
    public async Task<IssueResponse> Handle(IssueGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        Issue? issue = await wrapper.Issues.GetByIdAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"задача с id: {request.id} не найдена");

        return issue.ToResponse();
    }
}
