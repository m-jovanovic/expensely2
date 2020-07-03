using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;

namespace Expensely.Presentation.Middleware
{
    public class LocaleStorageMiddleware : Fluxor.Middleware
    {
        private readonly ILocalStorageService _localStorageService;
        private IStore? _store;

        public LocaleStorageMiddleware(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task InitializeAsync(IStore store)
        {
            _store = store;

            foreach (IFeature feature in _store.Features.Values)
            {
                if (feature.GetName().StartsWith("@"))
                {
                    continue;
                }

                string stateKey = $"State-{feature.GetName()}";

                bool keyExists = await _localStorageService.ContainKeyAsync(stateKey).ConfigureAwait(false);

                if (keyExists)
                {

                    byte[] stateBytes = await _localStorageService.GetItemAsync<byte[]>(stateKey).ConfigureAwait(false);

                    if (stateBytes != null)
                    {
                        object state = JsonSerializer.Deserialize(stateBytes, feature.GetStateType());

                        feature.RestoreState(state);
                    }
                }

                feature.StateChanged += async (sender, args) =>
                {
                    byte[] stateBytes = JsonSerializer.SerializeToUtf8Bytes(feature.GetState());

                    await _localStorageService.SetItemAsync(stateKey, stateBytes);
                };
            }

            await base.InitializeAsync(store).ConfigureAwait(false);
        }
    }

    public static class LocaleStorageMiddlewareExtensions
    {
        public static Fluxor.DependencyInjection.Options UseLocaleStorage(this Fluxor.DependencyInjection.Options options)
            => options.AddMiddleware<LocaleStorageMiddleware>();
    }
}
