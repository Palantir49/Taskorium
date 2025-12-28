using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskService.Domain.IRepositories;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly TaskServiceDbContext _context;

        protected RepositoryBase(TaskServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<T?> GetByIdAsync( Guid id, CancellationToken ct = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, ct);
        }

        public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity, ct);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        //public virtual async Task<T?> GetByConditionAsync(
        //    Expression<Func<T, bool>> predicate,
        //    CancellationToken ct = default)
        //{
        //    return await _context.Set<T>()
        //        .FirstOrDefaultAsync(predicate, ct);
        //}

        //public virtual async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        //{
        //    return await _context.Set<T>()
        //        .ToListAsync(ct);
        //}

        //public virtual async Task<List<T>> GetListByConditionAsync(
        //    Expression<Func<T, bool>> predicate,
        //    CancellationToken ct = default)
        //{
        //    return await _context.Set<T>()
        //        .Where(predicate)
        //        .ToListAsync(ct);
        //}
    }
}
