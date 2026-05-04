using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Read.Result
{
    public record class GetUserByIdResult(Guid? id, string? userEmail);
}
