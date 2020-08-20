using Expensely.Domain.Validators.Common;

namespace Expensely.Domain.Validators.LastName
{
    /// <summary>
    /// Validates that the last name is not longer than allowed.
    /// </summary>
    public sealed class LastNameMaxLengthValidator : StringMaxLengthValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameMaxLengthValidator"/> class.
        /// </summary>
        public LastNameMaxLengthValidator()
            : base(ValueObjects.LastName.MaxLength, Errors.LastName.LongerThanAllowed)
        {
        }
    }
}