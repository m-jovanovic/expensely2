using Expensely.Domain.ValueObjects;

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
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
