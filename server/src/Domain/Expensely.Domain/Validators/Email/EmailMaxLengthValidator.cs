using Expensely.Domain.Validators.Common;

namespace Expensely.Domain.Validators.Email
{
    /// <summary>
    /// Validates that the email is not longer than the maximum length.
    /// </summary>
    public sealed class EmailMaxLengthValidator : StringMaxLengthValidator
    {
        public EmailMaxLengthValidator()
            : base(ValueObjects.Email.MaxLength, Errors.Email.LongerThanAllowed)
        {
        }
    }
}
