namespace Expensely.Application.Messaging
{
    public interface ICacheableQuery<out TResponse> : IQuery<TResponse>
    {
        string GetCacheKey();

        int GetCacheTime() => 10;
    }
}
