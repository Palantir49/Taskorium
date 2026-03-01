using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Mapping;

public static class IssueTypeResponseMapping
{
    public static IssueTypeResponse ToResponse(this IssueType type)
    {
        return new IssueTypeResponse(
            Number: (int)type,
                Name: type.ToString(),
                DisplayName: GetDisplayName(type),
                Code: GetCode(type)
            );
    }

    private static string GetDisplayName(IssueType type)
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

    private static string GetCode(IssueType type)
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
