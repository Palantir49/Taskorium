using TaskService.Application.Commands.Users.Create;
using TaskService.Application.Commands.Users.Get;
using TaskService.Contracts.User.Requests;
using TaskService.Domain.ValueObjects;

namespace TaskService.Application.Commands.Users
{
    public static class UserMapping
    {
        public static CreateUserCommand ToCommand(this CreateUserRequest request)
        {
            return new CreateUserCommand(request.Name, request.KeycloakId, new EmailAdress(request.Email), new UserName(request.Username));
        }

        public static GetUserByIdQuery ToCommand(this GetUserRequest request)
        {
            return new GetUserByIdQuery(request.Id);
        }
    }
}
