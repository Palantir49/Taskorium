using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeGetByProjectIdHandler(IRepositoryWrapper wrapper) : IRequestHandler<IssueTypeGetByProjectIdQuery, IEnumerable<IssueTypeResponse>>
{
    public async Task<IEnumerable<IssueTypeResponse>> Handle(IssueTypeGetByProjectIdQuery request, CancellationToken cancellationToken = default)
    {
        List<IssueType> types = await wrapper.IssueType.GetByProjectIdAsync(request.id, cancellationToken);

        return types.Select(x => x.ToResponse());
    }
}
