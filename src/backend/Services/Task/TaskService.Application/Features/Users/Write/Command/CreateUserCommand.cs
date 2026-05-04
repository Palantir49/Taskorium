using TaskService.Application.Features.Users.Write.Result;
using TaskService.Application.Mediator;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Features.Users.Write.Command;

public record CreateUserCommand(
    string Name,
    Guid KeycloakId,
    EmailAdress Email,
    UserName Username) : ICommand<CreateUserResult>;
