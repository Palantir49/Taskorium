using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;
namespace TaskService.Application.Mapping;

public static class WorkspaceRoleMap
{
    extension(WorkspaceRoles domainEnum)
    {
        public WorkspaceRolesDto ToDto()
        {
            switch (domainEnum)
            {
                case WorkspaceRoles.Admin:
                    return WorkspaceRolesDto.Admin;

                case WorkspaceRoles.Creator:
                    return WorkspaceRolesDto.Creator;
                case WorkspaceRoles.Viewer:
                    return WorkspaceRolesDto.Viewer;
                default:
                    return WorkspaceRolesDto.Viewer;
            }
        }
    }
    extension(WorkspaceRolesDto domainEnum)
    {
        public WorkspaceRoles ToEntity()
        {
            switch (domainEnum)
            {
                case WorkspaceRolesDto.Admin:
                    return WorkspaceRoles.Admin;

                case WorkspaceRolesDto.Creator:
                    return WorkspaceRoles.Creator;
                case WorkspaceRolesDto.Viewer:
                    return WorkspaceRoles.Viewer;
                default:
                    return WorkspaceRoles.Viewer;
            }
        }
    }
}
