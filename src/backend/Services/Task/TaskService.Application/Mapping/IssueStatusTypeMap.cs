using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping
{
    public static class IssueStatusTypeMap
    {
        extension(IssueStatusType domainEnum)
        {
            public IssueStatusTypeDto ToDto()
            {
                switch (domainEnum)
                {
                    case IssueStatusType.Initial:
                        return IssueStatusTypeDto.Initial;
                    case IssueStatusType.Process:
                        return IssueStatusTypeDto.Process;
                    case IssueStatusType.Success:
                        return IssueStatusTypeDto.Success;
                    case IssueStatusType.Rejected:
                        return IssueStatusTypeDto.Rejected;
                    default:
                        return IssueStatusTypeDto.Initial; // Значение по умолчанию (можно изменить на выбрасывание исключения, если нужно)
                }
            }
        }
        extension(IssueStatusTypeDto dtoEnum)
        {
            public IssueStatusType ToEntity()
            {
                switch (dtoEnum)
                {
                    case IssueStatusTypeDto.Initial:
                        return IssueStatusType.Initial;
                    case IssueStatusTypeDto.Process:
                        return IssueStatusType.Process;
                    case IssueStatusTypeDto.Success:
                        return IssueStatusType.Success;
                    case IssueStatusTypeDto.Rejected:
                        return IssueStatusType.Rejected;
                    default:
                        return IssueStatusType.Initial; // Значение по умолчанию
                }
            }
        }
    }
}
