using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Get;

public record GetUsersPageQuery(int skip, int take) : IQuery<GetUsersPageResult>;
