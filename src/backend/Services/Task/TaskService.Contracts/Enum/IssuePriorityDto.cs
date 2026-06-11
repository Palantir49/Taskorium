using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskService.Contracts.Enum
{
    public enum IssuePriorityDto
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
}
