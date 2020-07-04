using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Extensions;
using Expensely.Application.Messaging;
using Expensely.Application.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Expensely.Application.Behaviours
{
    public sealed class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly CachingOptions _cachingOptions;

        public CachingBehaviour(ICacheService cacheService, IOptions<CachingOptions> cachingOptions)
        {
            _cacheService = cacheService;
            _cachingOptions = cachingOptions.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!request.IsCacheableQuery() || !_cachingOptions.Enabled)
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
