using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Commands.Users.Get
{
    public record class GetUserByIdResult(Guid? id, string? userEmail);
}
