using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Extensions;
using Expensely.Application.Messaging;
using MediatR;

namespace Expensely.Application.Behaviours
{
    public sealed class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;

        public CachingBehaviour(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!request.IsCacheableQuery())
            {
                return await next();
            }

            var cacheableQuery = request as ICacheableQuery<TResponse>;

            TResponse? cachedValue = _cacheService.GetValue<TResponse>(cacheableQuery!.GetCacheKey());

            if (cachedValue != null)
            {
                return cachedValue;
            }

            TResponse response = await next();

            _cacheService.SetValue(cacheableQuery.GetCacheKey(), response, cacheableQuery.GetCacheTime());

            return response;
        }
    }
}
