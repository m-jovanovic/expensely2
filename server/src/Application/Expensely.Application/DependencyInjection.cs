using System.Reflection;
using Expensely.Application.Behaviours;
using Expensely.Application.Options;
using Expensely.Application.Services;
using Expensely.Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CachingOptions>(configuration.GetSection(CachingOptions.SettingsKey));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

            services.AddTransient<IPasswordHashChecker, PasswordHashChecker>();
        }
    }
}
