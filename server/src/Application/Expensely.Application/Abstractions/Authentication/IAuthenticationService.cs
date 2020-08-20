using System.Threading.Tasks;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the authentication service interface.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user with the specified credentials, and returns a JWT so that the user can be logged in.
        /// </summary>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The result of the registration process containing a JWT or an error.</returns>
        Task<Result<TokenResponse>> RegisterAsync(string firstName, string lastName, Email email, Password password);

        /// <summary>
        /// Logs in the user with the specified credentials.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The result of the login process containing a JWT or an error.</returns>
        Task<Result<TokenResponse>> LoginAsync(string email, string password);
    }
}
