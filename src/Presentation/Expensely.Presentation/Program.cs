using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Expensely.Common.Authorization.Permissions;
using Expensely.Presentation.Services;
using Expensely.Presentation.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Presentation
{
    public static class Program
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

            builder.Services.AddStore();

            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            await builder.Build().RunAsync();
        }
    }
}
