using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.User.Responses
{
    public record UserResponse(Guid Id,
                               Guid KeycloakId,
                               string? Email = null,
                               string? UserName = null,
                               DateTimeOffset? CreatedAt = null);
}
