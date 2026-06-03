using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.User.Responses;

namespace TaskService.Application.Features.Users.Read.GetUserProjectsById
{
    public record GetUserProjectsByIdResult(ICollection<UserProjectsResponse> Projects);

}
