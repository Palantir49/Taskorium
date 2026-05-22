using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Read.GetUserById
{
    public record class GetUserByIdResult(Guid? id, string? userEmail);
}
