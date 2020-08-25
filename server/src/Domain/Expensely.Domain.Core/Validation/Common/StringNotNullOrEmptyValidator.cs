using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Validation.Common
{
    /// <summary>
    /// Validates that the string being validated is not null or empty.
    /// </summary>
    public abstract class StringNotNullOrEmptyValidator : Validator<string>
    {
        private readonly Error _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringNotNullOrEmptyValidator"/> class.
        /// </summary>
        /// <param name="error">The error to return in case the validation fails.</param>
        protected StringNotNullOrEmptyValidator(Error error) => _error = error;

        /// <inheritdoc />
        public sealed override Result Validate(string? item)
            => string.IsNullOrWhiteSpace(item) ? Result.Fail(_error) : base.Validate(item);
    }
}
