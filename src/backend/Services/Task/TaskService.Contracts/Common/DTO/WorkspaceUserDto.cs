using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO
{
    public record WorkspaceUserDto(Guid Id,
                           Guid KeycloakId,
                           RolesDto Role,
                           DateTimeOffset JoinedAt,
                           string? Email = null,
                           string? UserName = null);
}
