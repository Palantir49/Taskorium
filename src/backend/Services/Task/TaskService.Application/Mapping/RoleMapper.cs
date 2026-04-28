using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping
{
    public static class RoleMapper
    {
        extension(Roles domainEnum)
        {
            public RolesDto ToDto()
            {
                switch (domainEnum)
                {
                    case Roles.Admin:
                        return RolesDto.Admin;
                    case Roles.Member:
                        return RolesDto.Member;
                    case Roles.Creator:
                        return RolesDto.Creator;
                    case Roles.Viewer:
                        return RolesDto.Viewer;
                    default:
                        return RolesDto.Viewer;
                }
            }
        }
        extension(RolesDto domainEnum)
        {
            public Roles ToEntity()
            {
                switch (domainEnum)
                {
                    case RolesDto.Admin:
                        return Roles.Admin;
                    case RolesDto.Member:
                        return Roles.Member;
                    case RolesDto.Creator:
                        return Roles.Creator;
                    case RolesDto.Viewer:
                        return Roles.Viewer;
                    default:
                        return Roles.Viewer;
                }
            }
        }
    }
}
