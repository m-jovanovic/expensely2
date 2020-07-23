namespace Expensely.Application.Caching
{
    /// <summary>
    /// Represents the cache service interface.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets the cached value corresponding to the specified key.
        /// </summary>
        /// <typeparam name="T">The cached object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached value if it exists, otherwise null.</returns>
        T? GetValue<T>(string key)
            where T : class;

        /// <summary>
        /// Sets the specified value in the cache with the specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheTimeInMinutes">The caching time in minutes.</param>
        void SetValue(string key, object value, int cacheTimeInMinutes);

        /// <summary>
        /// Removes the value with the specified key from the cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void RemoveValue(string key);
    }
}