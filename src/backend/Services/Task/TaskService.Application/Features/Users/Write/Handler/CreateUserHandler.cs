using TaskService.Application.Exceptions;
using TaskService.Application.Features.Users.Write.Command;
using TaskService.Application.Features.Users.Write.Result;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Users.Write.Handler;

public class CreateUserHandler(TaskServiceDbContext context)
    : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        //проверим существует ли пользователь
        var user = await context.Users.FindAsync([command.KeycloakId], cancellationToken);

        if (user is not null)
        {
            throw new ConflictException("Пользователь уже существует");
        }

        var newUser = User.Create(command.KeycloakId, command.Username, command.Email, command.Name);

        await context.Users.AddAsync(newUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateUserResult(newUser.Id, newUser.Email.ToString());
        // return new UserResponse(id: user.Id, keycloakId: user.KeycloakId, email: user.Email.ToString(), username: user.Username.ToString(), createdAt: user.CreatedDate);
    }
}
