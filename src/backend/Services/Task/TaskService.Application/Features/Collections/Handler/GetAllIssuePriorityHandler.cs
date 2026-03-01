using TaskService.Application.Features.Collections.Mapping;
using TaskService.Application.Features.Collections.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Handler;

public class GetAllIssuePriorityHandler : IRequestHandler<GetAllIsssuePriorityQuery, IEnumerable<IssuePriorityResponse>>
{
    public async Task<IEnumerable<IssuePriorityResponse>> Handle(GetAllIsssuePriorityQuery request, CancellationToken cancellationToken = default)
    {
        return Enum.GetValues<IssuePriority>()
            .Cast<IssuePriority>()
            .Select(x => x.ToResponse())
            .ToList();
    }
}
