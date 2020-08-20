using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;
using Expensely.Domain.Users.Validators.Password;

namespace Expensely.Domain.Users
{
    /// <summary>
    /// Represents the password value object.
    /// </summary>
    public sealed class Password : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="value">The password value.</param>
        private Password(string value) => Value = value;

        /// <summary>
        /// Gets the password value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Password? password) => password?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Password"/> instance based on the specified value.
        /// </summary>
        /// <param name="password">The password value.</param>
        /// <returns>The result of the password creation process containing the password or an error.</returns>
        public static Result<Password> Create(string? password)
        {
            IValidator<string> validator = new PasswordNotNullOrEmptyValidator()
                .SetNext(new PasswordMinLengthValidator())
                .SetNext(new PasswordHasLowercaseLetterValidator())
                .SetNext(new PasswordHasUppercaseLetterValidator())
                .SetNext(new PasswordHasDigitValidator())
                .SetNext(new PasswordHasNonAlphanumericValidator());

            Result result = validator.Validate(password);

            if (result.IsFailure)
            {
                return Result.Fail<Password>(result.Error);
            }

            return Result.Ok(new Password(password!));
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
