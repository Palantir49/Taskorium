using System.Linq.Expressions;

namespace TaskService.Domain.Repositories;

public interface IRepositoryBase<T>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);

    Task<List<T>?> GetListByConditionAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default);

    Task<T?> GetByConditionAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default);


    //Task<List<T>> GetAllAsync(
    //    CancellationToken ct = default);

    //Task<List<T>> GetListByConditionAsync(
    //    Expression<Func<T, bool>> predicate,
    //    CancellationToken ct = default);
}
