using Expensely.Domain.Users;

namespace Expensely.Application.Abstractions.Cryptography
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

        /// <summary>
        /// Checks if the specified password hash matches the provided password hash.
        /// </summary>
        /// <param name="passwordHash">The password hash.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns>True if the password hashes match, otherwise false..</returns>
        bool VerifyPasswordHash(string passwordHash, string providedPassword);
    }
}
