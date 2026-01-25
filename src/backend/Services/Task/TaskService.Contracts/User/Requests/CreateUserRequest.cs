using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.User.Requests
{
    public record CreateUserRequest(string Name,
                                    Guid KeycloakId,
                                    string Email,
                                    string Username);
}
