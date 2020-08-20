using Expensely.Application.Core.Abstractions.Cryptography;
using Expensely.Domain.Users.Abstractions;

namespace Expensely.Application.Users.Services
{
    /// <summary>
    /// Represents the password hash checker.
    /// </summary>
    internal sealed class PasswordHashChecker : IPasswordHashChecker
    {
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHashChecker"/> class.
        /// </summary>
        /// <param name="passwordHasher">The password hasher.</param>
        public PasswordHashChecker(IPasswordHasher passwordHasher) => _passwordHasher = passwordHasher;

        /// <inheritdoc />
        public bool HashesMatch(string passwordHash, string providedPassword)
            => _passwordHasher.VerifyPasswordHash(passwordHash, providedPassword);
    }
}
