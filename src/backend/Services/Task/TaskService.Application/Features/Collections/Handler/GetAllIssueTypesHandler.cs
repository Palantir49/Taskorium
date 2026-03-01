using TaskService.Application.Features.Collections.Mapping;
using TaskService.Application.Features.Collections.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Handler
{
    public class GetAllIssueTypesHandler : IRequestHandler<GetAllIsssueTypesQuery, IEnumerable<Contracts.Collections.IssueTypeResponse>>
    {
        public async Task<IEnumerable<IssueTypeResponse>> Handle(GetAllIsssueTypesQuery request, CancellationToken cancellationToken = default)
        {
            return Enum.GetValues<IssueType>()
                .Cast<IssueType>()
                .Select(x => x.ToResponse())
                .ToList();
        }
    }
}
