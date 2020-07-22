using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    /// <summary>
    /// Validates that the email is not null or empty.
    /// </summary>
    public class EmailNotNullOrEmptyValidator : Validator<string>
    {
        /// <inheritdoc />
        public override Result Validate(string? item) =>
            string.IsNullOrWhiteSpace(item) ? Result.Fail(Errors.Email.NullOrEmpty) : base.Validate(item);
    }
}
