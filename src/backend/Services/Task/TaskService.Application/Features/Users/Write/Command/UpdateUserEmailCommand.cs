using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TaskService.Application.Features.Users.Write.Result;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Write.Command
{
    public record UpdateUserEmailCommand(Guid id, string email) : ICommand<UpdateUserEmailResult>;
}
