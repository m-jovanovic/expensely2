using Expensely.Domain.Core.Validation.Common;

namespace Expensely.Domain.Users.Validators.LastName
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
            : base(Users.LastName.MaxLength, Errors.LastName.LongerThanAllowed)
        {
        }
    }
}