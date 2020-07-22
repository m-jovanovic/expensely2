using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    /// <summary>
    /// Validates that the password is longer than the minimum length.
    /// </summary>
    public class PasswordMinLengthValidator : Validator<string>
    {
        /// <summary>
        /// The password minimum length.
        /// </summary>
        public const int MinPasswordLength = 6;

        /// <inheritdoc />
        public override Result Validate(string? item) =>
            item?.Length < MinPasswordLength ? Result.Fail(Errors.Password.TooShort) : base.Validate(item);
    }
}
