using System.Linq.Expressions;
using MongoDB.Driver;
using NotificationService.Domain.Aggregates.Abstracts;
using NotificationService.Domain.Aggregates.Outbox;
using NotificationService.Domain.Repositories.Interfaces;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.Abstracts;

public abstract class RepositoryBase<T>(NotificationDbContext context)
    : IRepository<T> where T : AggregateRoot
{
    //  private readonly IClientSessionHandle? session = context.CurrentSession;
    protected readonly IMongoCollection<T> Collection = GetCollection(context);


    public async Task<T?> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var session = context.CurrentSession;
        return session is not null
            ? await Collection.Find(session, filter).FirstOrDefaultAsync()
            : await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        var session = context.CurrentSession;
        if (session is not null)
        {
            await Collection.InsertOneAsync(session, entity);
        }
        else
        {
            await Collection.InsertOneAsync(entity);
        }
    }

    public async Task<List<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression)
    {
        var session = context.CurrentSession;
        return session is not null
            ? await Collection.Find(session, expression).ToListAsync()
            : await Collection.Find(expression).ToListAsync();
    }

    public async Task<bool> UpdateOneAsync(T entity)
    {
        var session = context.CurrentSession;
        var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);

        var result = session is not null
            ? await Collection.ReplaceOneAsync(session, filter, entity)
            : await Collection.ReplaceOneAsync(filter, entity);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(T entity)
    {
        var session = context.CurrentSession;
        var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);

        var result = session is not null
            ? await Collection.DeleteOneAsync(session, filter)
            : await Collection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    private static IMongoCollection<T> GetCollection(NotificationDbContext context)
    {
        var type = typeof(T);

        if (type == typeof(Domain.Aggregates.Notification.Notification))
        {
            return (IMongoCollection<T>)context.Notifications;
        }

        if (type == typeof(OutBoxMessage))
        {
            return (IMongoCollection<T>)context.OutBoxMessages;
        }

        throw new InvalidOperationException(
            $"Unknown collection for type {type.Name}");
    }
}
