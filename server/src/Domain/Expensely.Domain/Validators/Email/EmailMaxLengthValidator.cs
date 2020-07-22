using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    /// <summary>
    /// Validates that the email is not longer than the maximum length.
    /// </summary>
    public class EmailMaxLengthValidator : Validator<string>
    {
        /// <summary>
        /// The email maximum length.
        /// </summary>
        public const int MaxEmailLength = 256;

        /// <inheritdoc />
        public override Result Validate(string? item) =>
            item?.Length > MaxEmailLength ? Result.Fail(Errors.Email.LongerThanAllowed) : base.Validate(item);
    }
}
