using System;
using System.Collections.Concurrent;
using System.Linq;
using Expensely.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Expensely.Infrastructure.Services.Caching
{
    /// <summary>
    /// Represents the cache service.
    /// </summary>
    internal sealed class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new ConcurrentDictionary<string, bool>();
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        public CacheService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        /// <inheritdoc />
        public T? GetValue<T>(string key)
            where T : class
            => _memoryCache.TryGetValue(key, out T value) ? value : null;

        /// <inheritdoc />
        public void SetValue(string key, object value, int cacheTimeInMinutes)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTimeInMinutes)
            });

            CacheKeys.TryAdd(key, true);
        }

        /// <inheritdoc />
        public void RemoveValue(string key)
        {
            _memoryCache.Remove(key);

            CacheKeys.TryRemove(key, out _);
        }

        /// <inheritdoc />
        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return;
            }

            foreach (string cacheKey in CacheKeys.Keys.Where(cacheKey => cacheKey.StartsWith(pattern)))
            {
                RemoveValue(cacheKey);
            }
        }
    }
}
