using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.IRepositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
