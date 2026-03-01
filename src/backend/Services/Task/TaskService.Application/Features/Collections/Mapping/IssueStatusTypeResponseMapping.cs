using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Mapping;

public static class IssueStatusTypeResponseMapping
{
    public static IssueStatusTypeResponse ToResponse(this IssueStatusType type)
    {
        return new IssueStatusTypeResponse(
            Number: (int)type,
            Name: type.ToString(),
            DisplayName: GetDisplayName(type)
            );
    }

    private static string GetDisplayName(IssueStatusType type)
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
