using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    internal class UnitOfWork(TaskServiceDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await context.SaveChangesAsync();
        }
    }
}
