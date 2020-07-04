using Expensely.Application.Caching;
using Expensely.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
