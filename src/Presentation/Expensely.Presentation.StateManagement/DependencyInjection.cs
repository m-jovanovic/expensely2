using System.Reflection;
using Expensely.Presentation.StateManagement.Facades;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Presentation.StateManagement
{
    public static class DependencyInjection
    {
        public static void AddStateManagement(this IServiceCollection services)
        {
            services.AddFluxor(c =>
            {
                c.ScanAssemblies(Assembly.GetExecutingAssembly());
                c.UseRouting();
                c.UseReduxDevTools();
            });

            services.AddScoped<IExpensesFacade, ExpensesFacade>();
        }
    }
}
