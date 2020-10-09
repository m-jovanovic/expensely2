using Expensely.Domain.Users;

namespace Expensely.Application.Core.Abstractions.Cryptography
{
    /// <summary>
    /// Represents the password hasher interface.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed representation of the specified password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the specified password.</returns>
        string HashPassword(Password password);
    }
}
