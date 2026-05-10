using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Features.Users.Read.GetUserById
{
    public record GetUserByIdQuery(Guid id) : IQuery<GetUserByIdResult>;
}
