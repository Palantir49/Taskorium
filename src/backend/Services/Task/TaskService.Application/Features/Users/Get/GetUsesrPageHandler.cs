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
        .Select(x => new UserResponse(id: x.Id,
                                      keycloakId: x.KeycloakId,
                                      email: x.Email.Value,
                                      username: x.Username.Value,
                                      createdAt: x.CreatedDate))
        .ToListAsync();

        return new GetUsersPageResult(users: result);
    }
}
