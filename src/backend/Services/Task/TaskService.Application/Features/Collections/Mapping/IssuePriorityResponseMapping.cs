using System.ComponentModel;
using System.Reflection;
using TaskService.Contracts.Collections;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.Collections.Mapping;

public static class IssuePriorityResponseMapping
{
    public static IssuePriorityResponse ToResponse(this IssuePriority type)
    {
        return new IssuePriorityResponse(
            Number: (int)type,
                Name: type.ToString(),
                DisplayName: GetDisplayName(type)
            );
    }

    private static string GetDisplayName(this IssuePriority priority)
    {
        var type = priority.GetType();
        var member = type.GetMember(priority.ToString());
        var attribute = member[0].GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? priority.ToString();
    }
}
