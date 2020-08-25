using Expensely.Domain.Core.Validation.Common;

namespace Expensely.Domain.Users.Validators.LastName
{
    /// <summary>
    /// Validates that the last name is not null or empty.
    /// </summary>
    public sealed class LastNameNotNullOrEmptyValidator : StringNotNullOrEmptyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameNotNullOrEmptyValidator"/> class.
        /// </summary>
        public LastNameNotNullOrEmptyValidator()
            : base(Errors.LastName.NullOrEmpty)
        {
        }
    }
}