using System.Threading.Tasks;
using Blazored.LocalStorage;
using Expensely.Presentation.Services.Authentication;
using Expensely.Presentation.Services.Infrastructure;
using Expensely.Presentation.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Expensely.Presentation.Services.Implementations
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;

        public AuthenticationService(ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            AccessToken accessToken = await _localStorageService.GetItemAsync<AccessToken>(AccessToken.Key);

            return accessToken != null && !string.IsNullOrWhiteSpace(accessToken.Token);
        }

        public async Task SignInAsync(string email, string password)
        {
            var accessToken = new AccessToken
            {
                Token = email + password
            };

            await StoreAccessTokenAsync(accessToken);

            _navigationManager.NavigateTo(Routes.Home);
        }

        public async Task SignOutAsync()
        {
            await _localStorageService.RemoveItemAsync(AccessToken.Key);

            _navigationManager.NavigateTo(Routes.Login);
        }

        private async Task StoreAccessTokenAsync(AccessToken accessToken)
        {
            await _localStorageService.SetItemAsync(AccessToken.Key, accessToken);
        }
    }
}