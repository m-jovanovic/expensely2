using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages;
using Expensely.Presentation.Services.Infrastructure;
using Expensely.Presentation.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Expensely.Presentation.Infrastructure
{
    public class PageComponent : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILanguageContainerService LanguageContainerService { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        protected string Resource(string key) => LanguageContainerService.Keys[key];

        protected override async Task OnInitializedAsync()
        {
            bool isAuthenticated = await AuthenticationService.IsAuthenticatedAsync();

            if (!isAuthenticated)
            {
                NavigationManager.NavigateTo(Routes.Login);
            }

            await base.OnInitializedAsync();
        }
    }
}
