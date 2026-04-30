using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Responses;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Users.Get;

public class GetUsesrPageHandler(TaskServiceDbContext context) : IRequestHandler<GetUsersPageQuery, GetUsersPageResult>
{
    public async Task<GetUsersPageResult> Handle(GetUsersPageQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Users
        .AsNoTracking()
        .Skip(request.skip)
        .Take(request.take)
        .Select(x => new UserResponse(Id: x.Id,
                                      KeycloakId: x.KeycloakId,
                                      Email: x.Email.Value,
                                      UserName: x.Username.Value,
                                      CreatedAt: x.CreatedDate))
        .ToListAsync();

        return new GetUsersPageResult(users: result);
    }
}
