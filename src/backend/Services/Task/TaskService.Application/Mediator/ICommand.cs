using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Mediator;

public interface ICommand<out TResult> : IRequest<TResult>
{

}
