using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;
using Expensely.Domain.Validators.Password;

namespace Expensely.Domain.ValueObjects
{
    public class Password : ValueObject
    {
        private Password(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static implicit operator string(Password? password) => password?.Value ?? string.Empty;

        public static Result<Password> Create(string? password)
        {
            IValidator<string> validator = new PasswordNotNullOrEmptyValidator();
            validator
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

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
