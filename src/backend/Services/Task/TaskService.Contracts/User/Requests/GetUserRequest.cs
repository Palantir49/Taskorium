using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.User.Requests
{
    public record GetUserRequest(Guid Id);
}
