using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Caching;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Extensions;
using Expensely.Application.Core.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Expensely.Application.Core.Behaviours
{
    /// <summary>
    /// Represents the caching behaviour middleware.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    internal sealed class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly CachingOptions _cachingOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cachingOptions">The caching options.</param>
        public CachingBehaviour(ICacheService cacheService, IOptions<CachingOptions> cachingOptions)
        {
            _cacheService = cacheService;
            _cachingOptions = cachingOptions.Value;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
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
