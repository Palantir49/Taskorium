using TaskService.Application.Exceptions;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.Users.Create;

public class CreateUserHandler(IRepositoryWrapper wrapper) : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        //проверим существует ли пользователь
        var user = await wrapper.Users.GetByConditionAsync(x => x.KeycloakId == command.KeycloakId,
            ct: cancellationToken);

        if (user is not null)
        {
            throw new ConflictException("Пользователь уже существует");
        }

        var newUser = User.Create(command.KeycloakId, command.Username, command.Email, command.Name);

        await wrapper.Users.AddAsync(newUser, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return new CreateUserResult(newUser.Id, newUser.Email.ToString());
        // return new UserResponse(id: user.Id, keycloakId: user.KeycloakId, email: user.Email.ToString(), username: user.Username.ToString(), createdAt: user.CreatedDate);
    }
}
