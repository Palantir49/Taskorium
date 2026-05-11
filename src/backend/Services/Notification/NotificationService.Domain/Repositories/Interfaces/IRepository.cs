using System.Linq.Expressions;

namespace NotificationService.Domain.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
    Task<bool> UpdateOneAsync(T entity);
    Task<bool> DeleteOneAsync(T entity);
}
