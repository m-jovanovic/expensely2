using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    /// <summary>
    /// Validates that the password has at least one uppercase letter.
    /// </summary>
    public class PasswordHasUppercaseLetterValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            !item?.Any(IsUpper) ?? false ?
                Result.Fail(Errors.Password.MissingUppercaseLetter) : base.Validate(item);

        /// <summary>
        /// Checks if the specified character is uppercase.
        /// </summary>
        /// <param name="c">The character being checked.</param>
        /// <returns>True if the character is uppercase, otherwise false.</returns>
        private static bool IsUpper(char c) => c >= 'A' && c <= 'Z';
    }
}