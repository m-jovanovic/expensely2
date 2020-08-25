using Expensely.Domain.Core.Validation.Common;

namespace Expensely.Domain.Users.Validators.FirstName
{
    /// <summary>
    /// Validates that the first name is not null or empty.
    /// </summary>
    public sealed class FirstNameNotNullOrEmptyValidator : StringNotNullOrEmptyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameNotNullOrEmptyValidator"/> class.
        /// </summary>
        public FirstNameNotNullOrEmptyValidator()
            : base(Errors.FirstName.NullOrEmpty)
        {
        }
    }
}
