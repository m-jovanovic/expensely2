using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Expensely.Presentation.Services;
using Expensely.Presentation.StateManagement;
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

            builder.Services.AddBlazoredLocalStorage();
            
            builder.Services.AddServices();

            builder.Services.AddStateManagement();

            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

            await builder.Build().RunAsync();
        }
    }
}
