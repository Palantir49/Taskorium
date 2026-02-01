using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueStatuses.Handler
{
    public class IssueStatusGetByProjectIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueStatusGetByProjectIdQuery, IEnumerable<IssueStatusResponse>>
    {
        public async Task<IEnumerable<IssueStatusResponse>> Handle(IssueStatusGetByProjectIdQuery request, CancellationToken cancellationToken = default)
        {
            List<IssueStatus> statuses = await wrapper.IssueStatus.GetByProjectIdAsync(request.id, cancellationToken);

            return statuses.Select(x => x.ToResponse());
        }
    }
}
