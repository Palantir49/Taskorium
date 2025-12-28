using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
