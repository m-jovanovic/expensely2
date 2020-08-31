namespace Expensely.Domain.Core.Validation.Common
{
    /// <summary>
    /// Validates that the string being validated is not longer than allowed.
    /// </summary>
    public abstract class StringMaxLengthValidator : Validator<string>
    {
        private readonly int _maxLength;
        private readonly Error _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMaxLengthValidator"/> class.
        /// </summary>
        /// <param name="maxLength">The max length for the string.</param>
        /// <param name="error">The error to return in case the validation fails.</param>
        protected StringMaxLengthValidator(int maxLength, Error error)
        {
            _maxLength = maxLength;
            _error = error;
        }

        /// <inheritdoc />
        public sealed override Result Validate(string? item) =>
            item?.Length > _maxLength ? Result.Failure(_error) : base.Validate(item);
    }
}