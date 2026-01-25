using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private readonly IRepositoryWrapper _wrapper;
    public CreateUserHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = User.Create(command.KeycloakId, command.Username, command.Email);

        await _wrapper.Users.AddAsync(user, cancellationToken);
        await _wrapper.SaveChangesAsync(cancellationToken);

        return new CreateUserResult(id: user.Id, userEmail: user.Email.ToString());
        // return new UserResponse(id: user.Id, keycloakId: user.KeycloakId, email: user.Email.ToString(), username: user.Username.ToString(), createdAt: user.CreatedDate);
    }
}

