using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Write.UpdateUserEmail
{
    public record UpdateUserEmailCommand(Guid Id, string Email) : ICommand<UpdateUserEmailResult>;
}
