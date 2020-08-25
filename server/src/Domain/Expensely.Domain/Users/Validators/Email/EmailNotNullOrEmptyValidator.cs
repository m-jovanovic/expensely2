using Expensely.Domain.Core.Validation.Common;

namespace Expensely.Domain.Users.Validators.Email
{
    /// <summary>
    /// Validates that the email is not null or empty.
    /// </summary>
    public sealed class EmailNotNullOrEmptyValidator : StringNotNullOrEmptyValidator
    {
        public EmailNotNullOrEmptyValidator()
            : base(Errors.Email.NullOrEmpty)
        {
        }
    }
}
