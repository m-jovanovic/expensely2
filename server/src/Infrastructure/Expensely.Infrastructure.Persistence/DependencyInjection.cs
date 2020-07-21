using Expensely.Application.Abstractions;
using Expensely.Infrastructure.Persistence.Factories;
using Expensely.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(ConnectionString.ExpenselyDb);
            services.AddSingleton(new ConnectionString(connectionString));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddDbContextPool<ExpenselyDbContext>(options => options.UseNpgsql(connectionString).EnableSensitiveDataLogging());
            services.AddScoped<IDbContext>(impl => impl.GetRequiredService<ExpenselyDbContext>());
            services.AddScoped<IUnitOfWork>(impl => impl.GetRequiredService<ExpenselyDbContext>());
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
