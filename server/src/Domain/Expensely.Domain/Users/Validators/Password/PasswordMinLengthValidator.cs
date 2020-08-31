using Expensely.Domain.Core;
using Expensely.Domain.Core.Validation;

namespace Expensely.Domain.Users.Validators.Password
{
    /// <summary>
    /// Validates that the password is longer than the minimum length.
    /// </summary>
    public sealed class PasswordMinLengthValidator : Validator<string>
    {
        /// <summary>
        /// The password minimum length.
        /// </summary>
        public const int MinPasswordLength = 6;

        /// <inheritdoc />
        public override Result Validate(string? item) =>
            item?.Length < MinPasswordLength ? Result.Failure(Errors.Password.TooShort) : base.Validate(item);
    }
}
