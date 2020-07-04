using AKSoftware.Localization.MultiLanguages;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;

namespace Expensely.Presentation.Infrastructure
{
    public class StatefulComponent : FluxorComponent
    {
        [Inject]
        private ILanguageContainerService LanguageContainerService { get; set; }

        protected string Resource(string key) => LanguageContainerService.Keys[key];
    }
}
