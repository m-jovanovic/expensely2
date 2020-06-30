using System;
using Expensely.Application.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Expensely.Infrastructure.Caching
{
    public sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <inheritdoc />
        public T? GetValue<T>(string key, Func<T> factory)
            where T : class
        {
            bool cacheHit = _memoryCache.TryGetValue(key, out T value);

            if (cacheHit)
            {
                return value;
            }

            value = factory();

            SetValue(key, value);

            return value;
        }

        /// <inheritdoc />
        public void SetValue(string key, object value)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        /// <inheritdoc />
        public void RemoveValue(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
