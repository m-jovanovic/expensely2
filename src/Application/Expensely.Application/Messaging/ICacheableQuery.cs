namespace Expensely.Application.Messaging
{
    public interface ICacheableQuery<out TResponse> : IQuery<TResponse>
    {
        string CreateCacheKey();

        int GetCacheTime() => 10;
    }
}
