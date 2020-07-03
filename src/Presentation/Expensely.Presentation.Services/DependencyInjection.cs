using Expensely.Presentation.Services.Implementations;
using Expensely.Presentation.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Presentation.Services
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
        }
    }
}
