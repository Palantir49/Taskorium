using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.User.Responses;

namespace TaskService.Contracts.Project.Responses
{
    public record ProjectMembersResponse(Guid ProjectId, string ProjectName, IEnumerable<ProjectUserDto> Members);
}
