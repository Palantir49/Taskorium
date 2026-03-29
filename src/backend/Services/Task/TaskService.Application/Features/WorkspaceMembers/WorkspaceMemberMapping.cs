using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Mediator;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Features.WorkspaceMembers
{
    public static class WorkspaceMemberMapping
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
    }
}
