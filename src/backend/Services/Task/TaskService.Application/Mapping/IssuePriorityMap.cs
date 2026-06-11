using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping
{
    public static class IssuePriorityMap
    {
        extension(IssuePriority domainEnum)
        {
            public IssuePriorityDto ToDto()
            {
                switch (domainEnum)
                {
                    case IssuePriority.Critical:
                        return IssuePriorityDto.Critical;
                    case IssuePriority.Low:
                        return IssuePriorityDto.Low;
                    case IssuePriority.Medium:
                        return IssuePriorityDto.Medium;
                    case IssuePriority.High:
                        return IssuePriorityDto.High;
                    case IssuePriority.None:
                        return IssuePriorityDto.None;
                    default:
                        return IssuePriorityDto.None;
                }
            }
        }
        extension(IssuePriorityDto dtoEnum)
        {
            public IssuePriority ToEntity()
            {
                switch (dtoEnum)
                {
                    case IssuePriorityDto.Critical:
                        return IssuePriority.Critical;
                    case IssuePriorityDto.Low:
                        return IssuePriority.Low;
                    case IssuePriorityDto.Medium:
                        return IssuePriority.Medium;
                    case IssuePriorityDto.High:
                        return IssuePriority.High;
                    case IssuePriorityDto.None:
                        return IssuePriority.None;
                    default:
                        return IssuePriority.None;
                }
            }
        }
    }
}
