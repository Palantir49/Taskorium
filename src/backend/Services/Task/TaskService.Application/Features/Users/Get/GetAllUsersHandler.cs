using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mediator;
using TaskService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Application.Features.Users.Get;

public class GetAllUsersHandler(TaskServiceDbContext context) : IRequestHandler<GetAllUsersQuery, GetAllUsersResult>
{
    public async Task<GetAllUsersResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Users
            .AsNoTracking()
            .Skip(request.skip)
            .Take(request.take)
            .ToListAsync();

        return new GetAllUsersResult(users: result);
    }
}
