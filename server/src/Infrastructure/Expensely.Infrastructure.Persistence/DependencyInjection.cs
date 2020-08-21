using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Infrastructure.Persistence.Data;
using Expensely.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

            services.AddSingleton(new ConnectionString(connectionString));

            if (connectionString.Length > 0)
            {
                services.AddDbContext<ExpenselyDbContext>(options =>
                    options
                        .UseNpgsql(connectionString)
                        .UseSnakeCaseNamingConvention());
            }

            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            services.AddScoped<IDbExecutor, DbExecutor>();

            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<ExpenselyDbContext>());

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ExpenselyDbContext>());

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
