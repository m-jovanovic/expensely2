using System.Reflection;
using Expensely.Presentation.Store.ExpensesList.Facades;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Presentation.Store
{
    public static class DependencyInjection
    {
        public static void AddStore(this IServiceCollection services)
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
