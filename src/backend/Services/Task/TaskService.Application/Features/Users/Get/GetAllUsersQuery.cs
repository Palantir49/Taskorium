using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Get;

public record GetAllUsersQuery(int skip, int take) : IQuery<GetAllUsersResult>;
