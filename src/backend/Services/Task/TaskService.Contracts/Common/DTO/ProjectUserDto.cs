using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO;

public record ProjectUserDto(Guid UserId,
                           Guid KeycloakId,
                           ProjectRolesDto Role,
                           DateTimeOffset JoinedAt,
                           string? Email = null,
                           string? UserName = null);
