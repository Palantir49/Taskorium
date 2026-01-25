using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Commands.Users.Create
{
    public record CreateUserCommand(string Name,
                                    Guid KeycloakId,
                                    EmailAdress Email,
                                    UserName Username): ICommand<CreateUserResult>;
}
