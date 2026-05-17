using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping
{
    public static class ProjectRoleMap
    {
        extension(ProjectRoles domainEnum)
        {
            public ProjectRolesDto ToDto()
            {
                switch (domainEnum)
                {
                    case ProjectRoles.Admin:
                        return ProjectRolesDto.Admin;
                    case ProjectRoles.Member:
                        return ProjectRolesDto.Member;
                    case ProjectRoles.Creator:
                        return ProjectRolesDto.Creator;
                    case ProjectRoles.Viewer:
                        return ProjectRolesDto.Viewer;
                    default:
                        return ProjectRolesDto.Viewer;
                }
            }
        }
        extension(ProjectRolesDto domainEnum)
        {
            public ProjectRoles ToEntity()
            {
                switch (domainEnum)
                {
                    case ProjectRolesDto.Admin:
                        return ProjectRoles.Admin;
                    case ProjectRolesDto.Member:
                        return ProjectRoles.Member;
                    case ProjectRolesDto.Creator:
                        return ProjectRoles.Creator;
                    case ProjectRolesDto.Viewer:
                        return ProjectRoles.Viewer;
                    default:
                        return ProjectRoles.Viewer;
                }
            }
        }
    }
}
