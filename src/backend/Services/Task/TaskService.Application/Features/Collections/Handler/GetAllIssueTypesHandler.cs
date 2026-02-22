using TaskService.Application.Features.Collections.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Handler
{
    public class GetAllIssueTypesHandler : IRequestHandler<GetAllIsssueTypesQuery, IEnumerable<IssueTypeResponse>>
    {
        public async Task<IEnumerable<IssueTypeResponse>> Handle(GetAllIsssueTypesQuery request, CancellationToken cancellationToken = default)
        {
            return Enum.GetValues<IssueType>()
                .Cast<IssueType>()
                .Select(x => new IssueTypeResponse(
                    Number: (int)x,
                    Name: x.ToString(),
                    DisplayName: GetDisplayName(x),
                    Code: GetCode(x)
                    ))
                .ToList();
        }

        private string GetDisplayName(IssueType type)
        {
            return type switch
            {
                IssueType.Task => "Задача",
                IssueType.Story => "История (Фича)",
                IssueType.Bug => "Баг",
                IssueType.Improvenet => "Улучшение",
                IssueType.Epic => "Эпик",
                IssueType.TechDebt => "Технический долг",
                IssueType.Spike => "Исследование",
                IssueType.Documentation => "Документация",
                IssueType.Test => "Тест",
                _ => type.ToString()
            };
        }

        private string GetCode(IssueType type)
        {
            return type switch
            {
                IssueType.Task => "TSK",
                IssueType.Story => "STY",
                IssueType.Bug => "BUG",
                IssueType.Improvenet => "IMP",
                IssueType.Epic => "EPC",
                IssueType.TechDebt => "TD",
                IssueType.Spike => "SPK",
                IssueType.Documentation => "DOC",
                IssueType.Test => "TST",
                _ => type.ToString()
            };
        }
    }
}
