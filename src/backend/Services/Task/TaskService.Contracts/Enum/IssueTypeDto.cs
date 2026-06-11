using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Enum;

public enum IssueTypeDto
{
    Task = 0,
    Story = 1,
    Bug = 2,
    Improvement = 3,
    Epic = 4,
    TechDebt = 5,
    Spike = 6,
    Documentation = 7,
    Test = 8
}
