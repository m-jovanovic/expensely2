using System;
using Expensely.Application.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Expensely.Infrastructure.Caching
{
    public sealed class Cache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache)
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
