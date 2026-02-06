using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Commands.Users.Get;

public class GetUserByIdHandler(TaskServiceDbContext context) : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{

    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery command, CancellationToken cancellationToken = default)
    {

        var user = await context.Users.FindAsync(command.id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return new GetUserByIdResult(id: user.Id, userEmail: user.Email.ToString());
    }
}

