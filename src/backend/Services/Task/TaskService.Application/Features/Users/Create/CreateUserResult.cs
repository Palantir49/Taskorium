using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Commands.Users.Create
{
    public record class CreateUserResult(Guid id, string userEmail);
}
