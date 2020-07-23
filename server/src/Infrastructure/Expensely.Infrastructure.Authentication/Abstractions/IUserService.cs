using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Authentication.Entities;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    /// <summary>
    /// Represents the user service interface.
    /// </summary>
    internal interface IUserService
    {
        /// <summary>
        /// Gets the user with the specified email.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>The user with the specified email if it exists, otherwise null.</returns>
        Task<AuthenticatedUser?> GetByEmailAsync(string email);

        /// <summary>
        /// Creates the user with the specified parameters, and returns it.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result of the creation process containing the user or an error.</returns>
        Task<Result<AuthenticatedUser>> CreateAsync(string firstName, string lastName, Email email, Password password);
    }
}
