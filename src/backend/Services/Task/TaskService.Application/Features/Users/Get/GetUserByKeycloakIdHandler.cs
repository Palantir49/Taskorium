using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskService.Application.Features.Users.Get
{
    public class GetUserByKeycloakIdHandler(TaskServiceDbContext context) : IRequestHandler<GetUserByKeycloakIdQuery, GetUserByKeycloakIdResult>
    {
        public async Task<GetUserByKeycloakIdResult> Handle(GetUserByKeycloakIdQuery query, CancellationToken cancellationToken = default)
        {
            var existUser = await context.Users
                .Include(x => x.WorkspaceMembers)
                .Include(x => x.ProjectMembers)
                .FirstOrDefaultAsync(x => x.KeycloakId == query.keycloakId, cancellationToken);

            if (existUser is null)
            {
                throw new ArgumentNullException("Пользователь с таким keycloak id не существует", nameof(query.keycloakId));
            }

            var userWorkspaces = existUser.WorkspaceMembers
                .Select(x => new WorkSpaceMemberDto(x.WorkspaceId,
                                                    x.UserId,
                                                    new RoleDto(x.Role.ToString())))
                .ToList();

            var userProjects = existUser.ProjectMembers
                .Select(x => new ProjectMemberDto(x.ProjectId,
                                                    x.UserId,
                                                    new RoleDto(x.Role.ToString())))
                .ToList();

            return new GetUserByKeycloakIdResult(existUser.Id, existUser.KeycloakId, userProjects, userWorkspaces);
        }
    }
}
