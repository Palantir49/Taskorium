using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Users.Read.Result
{
    public record GetUserProjectsByIdResult(ICollection<ProjectResponse> Projects);

}
