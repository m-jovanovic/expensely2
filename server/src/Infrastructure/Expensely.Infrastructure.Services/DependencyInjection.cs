using Expensely.Application.Caching;
using Expensely.Infrastructure.Services.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure.Services
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
