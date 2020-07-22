using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;
using Expensely.Domain.Validators.Email;

namespace Expensely.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static implicit operator string(Email? email) => email?.Value ?? string.Empty;

        public static explicit operator Email(string email) => Create(email).Value();

        public static Result<Email> Create(string? email)
        {
            IValidator<string> validator = new EmailNotNullOrEmptyValidator();
            validator
                .SetNext(new EmailMaxLengthValidator())
                .SetNext(new EmailFormatValidator());

            Result result = validator.Validate(email);

            if (result.IsFailure)
            {
                return Result.Fail<Email>(result.Error);
            }

            return Result.Ok(new Email(email!));
        }

        internal static Email Empty => new Email(string.Empty);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
