using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    /// <summary>
    /// Validates that the password is not null or empty.
    /// </summary>
    public sealed class PasswordNotNullOrEmptyValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            string.IsNullOrWhiteSpace(item) ? Result.Fail(Errors.Password.NullOrEmpty) : base.Validate(item);
    }
}
