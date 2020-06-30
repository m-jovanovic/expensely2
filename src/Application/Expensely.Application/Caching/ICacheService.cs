using System;

namespace Expensely.Application.Caching
{
    public interface ICacheService
    {
        T? GetValue<T>(string key, Func<T> factory)
            where T : class;

        void SetValue(string key, object value);

        void RemoveValue(string key);
    }
}