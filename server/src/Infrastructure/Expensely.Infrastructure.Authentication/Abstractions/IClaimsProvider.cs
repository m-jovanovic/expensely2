using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Domain.Entities;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    /// <summary>
    /// Represents the claims provider interface.
    /// </summary>
    internal interface IClaimsProvider
    {
        /// <summary>
        /// Gets the claims for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The array of claims for the specified user.</returns>
        Task<Claim[]> GetClaimsAsync(User user);
    }
}