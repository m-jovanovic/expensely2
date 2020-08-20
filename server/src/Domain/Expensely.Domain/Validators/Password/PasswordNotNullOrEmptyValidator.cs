using Expensely.Domain.Validators.Common;

namespace Expensely.Domain.Validators.Password
{
    /// <summary>
    /// Validates that the password is not null or empty.
    /// </summary>
    public sealed class PasswordNotNullOrEmptyValidator : StringNotNullOrEmptyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordNotNullOrEmptyValidator"/> class.
        /// </summary>
        public PasswordNotNullOrEmptyValidator()
            : base(Errors.Password.NullOrEmpty)
        {
        }
    }
}
