using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.User.Responses
{
    public record UserResponse(Guid id,
                               Guid keycloakId,
                               string? email = null,
                               string? username = null,
                               DateTimeOffset? createdAt = null);
}
