using Expensely.Application.Abstractions.Caching;
using Expensely.Application.Abstractions.Common;
using Expensely.Infrastructure.Services.Caching;
using Expensely.Infrastructure.Services.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure.Services
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheService, CacheService>();

            services.AddTransient<IDateTime, MachineDateTime>();
        }
    }
}
