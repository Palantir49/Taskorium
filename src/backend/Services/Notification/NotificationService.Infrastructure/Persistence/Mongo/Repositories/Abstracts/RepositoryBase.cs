using System.Linq.Expressions;
using MongoDB.Driver;
using NotificationService.Domain.Repositories.Interfaces;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.Abstracts;

public abstract class RepositoryBase<T>(NotificationDbContext context)
    : IRepository<T> where T : Domain.Aggregates.Notification.Notification
{
    private readonly IMongoCollection<T> _collection = GetCollection(context);

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _collection.Find(x => x.EventIdempotencyKey.Value == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        await _collection.Find(expression).ToListAsync();
    }

    public async Task<bool> UpdateOneAsync(T entity)
    {
        var result = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(T entity)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == entity.Id);
        return result.DeletedCount > 0;
    }


    private static IMongoCollection<T> GetCollection(NotificationDbContext context)
    {
        var type = typeof(T);

        if (type == typeof(Domain.Aggregates.Notification.Notification))
        {
            return (IMongoCollection<T>)context.Notifications;
        }

        throw new InvalidOperationException($"Unknown collection for type {type.Name}");
    }
}
