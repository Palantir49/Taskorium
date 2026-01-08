using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    public class UserRepository(TaskServiceDbContext context) : RepositoryBase<User>(context), IUserRepository
    {

    }
}
