using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Delete
{
    public record DeleteUserByIdCommand(Guid id) : ICommand<DeleteUserByIdResult>;
}
