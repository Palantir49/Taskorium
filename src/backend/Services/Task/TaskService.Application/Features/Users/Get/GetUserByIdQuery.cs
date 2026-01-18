using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Commands.Users.Get
{
    public record GetUserByIdQuery(Guid id);
}
