using Expensely.Application.Interfaces;
using Expensely.Persistence.Factories;
using Expensely.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ExpenselyDb");
            services.AddSingleton(new ConnectionString(connectionString));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddDbContextPool<ExpenselyDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbContext>(impl => impl.GetRequiredService<ExpenselyDbContext>());
            services.AddScoped<IUnitOfWork>(impl => impl.GetRequiredService<ExpenselyDbContext>());
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
        }
    }
}
