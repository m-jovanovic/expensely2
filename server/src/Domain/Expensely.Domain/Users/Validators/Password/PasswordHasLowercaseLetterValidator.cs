using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;

namespace Expensely.Domain.Users.Validators.Password
{
    /// <summary>
    /// Validates that the password has at least one lowercase letter.
    /// </summary>
    public sealed class PasswordHasLowercaseLetterValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            !item?.Any(IsLower) ?? false ?
                Result.Fail(Errors.Password.MissingLowercaseLetter) : base.Validate(item);

        /// <summary>
        /// Checks if the specified character is lowercase.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is lowercase, otherwise false.</returns>
        private static bool IsLower(char c) => c >= 'a' && c <= 'z';
    }
}