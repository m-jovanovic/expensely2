using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Presentation.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            });

            builder.Services.AddFluxor(c =>
            {
                c.ScanAssemblies(typeof(Program).Assembly);
                c.UseRouting();
                c.UseReduxDevTools();
            });
            
            builder.Services.AddScoped<ExpenseService>();

            await builder.Build().RunAsync();
        }
    }
}
