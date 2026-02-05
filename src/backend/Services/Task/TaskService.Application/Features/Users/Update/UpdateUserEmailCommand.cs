using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Update
{
    public record UpdateUserEmailCommand(Guid id,string email):ICommand<UpdateUserEmailResult>;
}
