using System.Linq;
using System.Threading.Tasks;
using Expensely.Authentication.Interfaces;
using Expensely.Contracts.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Expensely.Authentication.Implementations
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<RegisterUserResponse> RegisterUserAsync(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return new RegisterUserResponse(true, null);
            }

            string[] errorCodes = result.Errors.Select(x => x.Code).ToArray();

            return new RegisterUserResponse(false, errorCodes);
        }
    }
}