namespace Expensely.Application.Caching
{
    public interface ICache
    {
        T? GetValue<T>(string key)
            where T : class;

        void SetValue(string key, object value);

        void RemoveValue(string key);
    }
}