using System;
using Expensely.Application.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Expensely.Infrastructure.Services.Caching
{
    public sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

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
