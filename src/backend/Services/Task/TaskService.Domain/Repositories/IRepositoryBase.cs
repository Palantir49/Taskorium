using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TaskService.Domain.Repositories;

public interface IRepositoryBase<T>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);

    //FAQ: добавляем ExistsByIdAsync? тип чтоб сразу проверка на месте была

    //Task<T?> GetByConditionAsync(
    //    Expression<Func<T, bool>> predicate,
    //    CancellationToken ct = default);

    //Task<List<T>> GetAllAsync(
    //    CancellationToken ct = default);

    //Task<List<T>> GetListByConditionAsync(
    //    Expression<Func<T, bool>> predicate,
    //    CancellationToken ct = default);
}
