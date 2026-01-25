
using TaskService.Application.Mediator;

using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Users.Get;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    private readonly IRepositoryWrapper _wrapper;
    public GetUserByIdHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery command, CancellationToken cancellationToken = default)
    {

        var user = await _wrapper.Users.GetByIdAsync(command.id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return new GetUserByIdResult(id: user.Id, userEmail: user.Email.ToString());
    }
}

