using System.ComponentModel;

namespace TaskService.Domain.Entities.Enums;

public enum IssuePriority
{
    [Description("Нет приоритета")]
    None = 0,

    [Description("Низкий")]
    Low = 1,

    [Description("Средний")]
    Medium = 2,

    [Description("Высокий")]
    High = 3,

    [Description("Критический")]
    Critical = 4
}
