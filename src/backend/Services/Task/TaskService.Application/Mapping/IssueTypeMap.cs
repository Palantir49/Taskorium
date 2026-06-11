using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping
{
    public static class IssueTypeMap
    {
        extension(IssueType domainEnum)
        {
            public IssueTypeDto ToDto()
            {
                switch (domainEnum)
                {
                    case IssueType.Task:
                        return IssueTypeDto.Task;
                    case IssueType.Story:
                        return IssueTypeDto.Story;
                    case IssueType.Bug:
                        return IssueTypeDto.Bug;
                    case IssueType.Improvement:
                        return IssueTypeDto.Improvement;
                    case IssueType.Epic:
                        return IssueTypeDto.Epic;
                    case IssueType.TechDebt:
                        return IssueTypeDto.TechDebt;
                    case IssueType.Spike:
                        return IssueTypeDto.Spike;
                    case IssueType.Documentation:
                        return IssueTypeDto.Documentation;
                    case IssueType.Test:
                        return IssueTypeDto.Test;
                    default:
                        return IssueTypeDto.Task;
                }
            }
        }

        extension(IssueTypeDto dtoEnum)
        {
            public IssueType ToEntity()
            {
                switch (dtoEnum)
                {
                    case IssueTypeDto.Task:
                        return IssueType.Task;
                    case IssueTypeDto.Story:
                        return IssueType.Story;
                    case IssueTypeDto.Bug:
                        return IssueType.Bug;
                    case IssueTypeDto.Improvement:
                        return IssueType.Improvement;
                    case IssueTypeDto.Epic:
                        return IssueType.Epic;
                    case IssueTypeDto.TechDebt:
                        return IssueType.TechDebt;
                    case IssueTypeDto.Spike:
                        return IssueType.Spike;
                    case IssueTypeDto.Documentation:
                        return IssueType.Documentation;
                    case IssueTypeDto.Test:
                        return IssueType.Test;
                    default:
                        return IssueType.Task;
                }
            }
        }
    }
}
