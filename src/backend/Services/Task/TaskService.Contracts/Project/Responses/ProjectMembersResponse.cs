using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Common.DTO;

namespace TaskService.Contracts.Project.Responses
{
    public record ProjectMembersResponse(Guid ProjectId, string ProjectName, IEnumerable<ProjectMemberDto> Members);
}
