using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Read.GetProjectMembers
{
    public record GetProjectMembersQuery(Guid Id) : IRequest<ProjectMembersResponse>;
}
