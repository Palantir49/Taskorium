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
            .Select(x => new IssueStatusTypeResponse(
                Number: (int)x,
                Name: x.ToString(),
                DisplayName: GetDisplayName(x)
                ))
            .ToList();
    }

    private string GetDisplayName(IssueStatusType type)
    {
        return type switch
        {
            IssueStatusType.Initial => "Новая",
            IssueStatusType.Process => "В работе",
            IssueStatusType.Success => "Выполнено",
            IssueStatusType.Rejected => "Отменено",
            _ => type.ToString()
        };
    }
}
