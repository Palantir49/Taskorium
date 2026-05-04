using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Features.Users.Read.Result;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Features.Users.Read.Query
{
    public record GetUserByIdQuery(Guid id) : IQuery<GetUserByIdResult>;
}
