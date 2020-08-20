using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;

namespace Expensely.Domain.Users.Validators.Password
{
    /// <summary>
    /// Validates that the password has at least one non-alphanumeric character.
    /// </summary>
    public sealed class PasswordHasNonAlphanumericValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            item?.All(IsLetterOrDigit) ?? false ? Result.Fail(Errors.Password.MissingNonAlphaNumeric) : base.Validate(item);

        /// <summary>
        /// Checks if the specified character is non-alphanumeric.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is uppercase, otherwise false.</returns>
        private static bool IsLetterOrDigit(char c) => IsLower(c) || IsUpper(c) || IsDigit(c);

        /// <summary>
        /// Checks if the specified character is lowercase.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is lowercase, otherwise false.</returns>
        private static bool IsLower(char c) => c >= 'a' && c <= 'z';

        /// <summary>
        /// Checks if the specified character is uppercase.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is uppercase, otherwise false.</returns>
        private static bool IsUpper(char c) => c >= 'A' && c <= 'Z';

        /// <summary>
        /// Checks if the specified character is an digit.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        private static bool IsDigit(char c) => c >= '0' && c <= '9';
    }
}