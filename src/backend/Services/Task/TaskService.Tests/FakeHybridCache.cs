using Microsoft.Extensions.Caching.Hybrid;

namespace TaskService.Tests;

public class FakeHybridCache : HybridCache
{
    public override ValueTask<T> GetOrCreateAsync<TState, T>(string key, TState state, Func<TState, CancellationToken, ValueTask<T>> factory, HybridCacheEntryOptions? options = null, IEnumerable<string>? tags = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Кэш не должен вызываться в тестах бизнес-логики!");
    }

    public override ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Кэш не должен вызываться в тестах бизнес-логики!");
    }

    public override ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Кэш не должен вызываться в тестах бизнес-логики!");
    }

    public override ValueTask SetAsync<T>(string key, T value, HybridCacheEntryOptions? options = null, IEnumerable<string>? tags = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Кэш не должен вызываться в тестах бизнес-логики!");
    }
}
