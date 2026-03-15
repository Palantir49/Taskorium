using TaskService.Application.Features.Collections.Mapping;
using TaskService.Application.Features.Collections.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Handler;


public class GetAllIssueStatusTypeHandler : IRequestHandler<GetAllIssueStatusTypesQuery, IEnumerable<IssueStatusTypeResponse>>
{
    public async Task<IEnumerable<IssueStatusTypeResponse>> Handle(GetAllIssueStatusTypesQuery request, CancellationToken cancellationToken = default)
    {
        return Enum.GetValues<IssueStatusType>()
            .Cast<IssueStatusType>()
            .Select(x => x.ToResponse())
            .ToList();
    }
}
