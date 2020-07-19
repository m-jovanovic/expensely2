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

        public static Result<Email> Create(string? email)
        {
            IValidator<string> validator = new EmailNullOrEmptyValidator();
            validator
                .SetNext(new EmailLengthValidator())
                .SetNext(new EmailFormatValidator());

            Result result = validator.Validate(email ?? string.Empty);

            if (result.IsFailure)
            {
                return Result.Fail<Email>(result.Error);
            }

            return Result.Ok(new Email(email!));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
