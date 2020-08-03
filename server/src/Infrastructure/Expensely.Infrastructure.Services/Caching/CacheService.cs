using System;
using Expensely.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Expensely.Infrastructure.Services.Caching
{
    /// <summary>
    /// Represents the cache service.
    /// </summary>
    internal sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        public CacheService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        /// <inheritdoc />
        public T? GetValue<T>(string key)
            where T : class
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : null;
        }

        /// <inheritdoc />
        public void SetValue(string key, object value, int cacheTimeInMinutes)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTimeInMinutes)
            });
        }

        /// <inheritdoc />
        public void RemoveValue(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
