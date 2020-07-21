using Expensely.Infrastructure.Authentication.Cryptography;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    /// <summary>
    /// Represents the password hasher interface.
    /// </summary>
    internal interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed representation of the specified password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the specified password.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
