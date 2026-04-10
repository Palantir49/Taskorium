using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Application.Features.WorkspaceMembers.GetAllUsers
{
    public class GetWorkspaceMembersHandler(TaskServiceDbContext context) : IRequestHandler<GetWorkspaceMembersQuery, GetWorkspaceMembersResult>
    {
        public async Task<GetWorkspaceMembersResult> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken = default)
        {
            var existUser = context.Users.Include(user => user.WorkspaceMembers).FirstOrDefault(x => x.KeycloakId == request.KeyoclakId);
            if (existUser is null)
            {
                throw new ArgumentNullException("Пользователь с таким keycloak id не существует",
                nameof(request.KeyoclakId));
            }
            var workspaceMembersDto = existUser.WorkspaceMembers.Select(x => new WorkSpaceMemberDto(WorkspaceId: x.WorkspaceId,
                                                                                                    UserId: x.UserId,
                                                                                                    Role: x.Role.ToDto()));

            throw new NotImplementedException();
        }
    }
}
