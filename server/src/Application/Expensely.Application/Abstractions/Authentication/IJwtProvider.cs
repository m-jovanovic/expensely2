using System.Threading.Tasks;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain.Users;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the JWT provider interface.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Creates the JWT for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT for the specified user.</returns>
        Task<TokenResponse> CreateAsync(User user);
    }
}
