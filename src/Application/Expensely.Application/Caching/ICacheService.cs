using System;

namespace Expensely.Application.Caching
{
    public interface ICacheService
    {
        T? GetValue<T>(string key)
            where T : class;

        void SetValue(string key, object value, int cacheTimeInMinutes);

        void RemoveValue(string key);
    }
}