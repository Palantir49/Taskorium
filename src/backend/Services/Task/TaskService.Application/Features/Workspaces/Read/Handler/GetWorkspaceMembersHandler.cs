using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Read.Query;
using TaskService.Application.Features.Workspaces.Read.Query;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Workspaces.Read.Handler
{
    public class GetWorkspaceMembersHandler(TaskServiceDbContext context, HybridCache cache)
        : IRequestHandler<GetWorkspaceMembersQuery, WorkspaceMembersResponse>
    {
        public async Task<WorkspaceMembersResponse> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"workspaceMembers_{request.Id}";

            return await cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var existProject = await context.Workspaces.Include(x => x.WorkspaceMembers)
                                                         .ThenInclude(x => x.User)
                                                         .AsSplitQuery()
                                                         .AsNoTracking()
                                                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (existProject is null)
                    throw new ArgumentNullException($"Рабочая область с id: {request.Id} не найден");

                var members = new List<WorkspaceUserDto>();

                foreach (var x in existProject.WorkspaceMembers)
                {
                    members.Add(new WorkspaceUserDto(Id: x.User.Id, KeycloakId: x.User.KeycloakId, Role: x.Role.ToDto(), JoinedAt: x.JoinedAt, Email: x.User.Email.Value, UserName: x.User.Username.Value));
                }

                return new WorkspaceMembersResponse(WorkspaceId: existProject.Id, WorkspaceName: existProject.Name.Value, Members: members);

            }, cancellationToken: cancellationToken);
        }
    }
}
