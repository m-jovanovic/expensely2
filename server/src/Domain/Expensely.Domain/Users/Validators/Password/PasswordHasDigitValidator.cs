using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;

namespace Expensely.Domain.Users.Validators.Password
{
    /// <summary>
    /// Validates that the password has at least one digit.
    /// </summary>
    public sealed class PasswordHasDigitValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            !item?.Any(IsDigit) ?? false ? Result.Fail(Errors.Password.MissingDigit) : base.Validate(item);

        /// <summary>
        /// Checks if the specified character is an digit.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        private static bool IsDigit(char c) => c >= '0' && c <= '9';
    }
}