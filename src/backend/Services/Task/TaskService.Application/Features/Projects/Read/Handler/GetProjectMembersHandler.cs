using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Projects.Read.Query;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Project.Responses;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Projects.Read.Handler
{
    public class GetProjectMembersHandler(TaskServiceDbContext context, HybridCache cache)
        : IRequestHandler<GetProjectMembersQuery, ProjectMembersResponse>
    {
        public async Task<ProjectMembersResponse> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"projectMembers_{request.Id}";

            return await cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var existProject = await context.Projects.Include(x => x.ProjectMembers)
                                                         .Include(x => x.Issues)
                                                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (existProject is null)
                    throw new ArgumentNullException($"Проект с id: {request.Id} не найден");

                var members = new List<ProjectMemberDto>();

                foreach (var x in existProject.ProjectMembers)
                {
                    members.Add(new ProjectMemberDto(ProjectId: existProject.Id, UserId: x.UserId, Role: x.Role.ToDto()));
                }

                return new ProjectMembersResponse(ProjectId: existProject.Id, ProjectName: existProject.Name.Value, Members: members);

            }, cancellationToken: cancellationToken);
        }
    }
}
