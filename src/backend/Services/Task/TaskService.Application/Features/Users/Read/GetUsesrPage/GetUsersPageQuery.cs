using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Users.Read.GetUsesrPage;

public record GetUsersPageQuery(int skip, int take) : IQuery<GetUsersPageResult>;
