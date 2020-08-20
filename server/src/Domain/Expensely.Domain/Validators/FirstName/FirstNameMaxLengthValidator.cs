using Expensely.Domain.Validators.Common;

namespace Expensely.Domain.Validators.FirstName
{
    /// <summary>
    /// Validates that the first name is not longer than allowed.
    /// </summary>
    public sealed class FirstNameMaxLengthValidator : StringMaxLengthValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameMaxLengthValidator"/> class.
        /// </summary>
        public FirstNameMaxLengthValidator()
            : base(ValueObjects.FirstName.MaxLength, Errors.FirstName.LongerThanAllowed)
        {
        }
    }
}