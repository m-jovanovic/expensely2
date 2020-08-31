﻿using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;
using Expensely.Domain.Users.Validators.Email;

namespace Expensely.Domain.Users
{
    /// <summary>
    /// Represents the email value object.
    /// </summary>
    public sealed class Email : ValueObject
    {
        /// <summary>
        /// The email maximum length.
        /// </summary>
        public const int MaxLength = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="value">The email value.</param>
        private Email(string value) => Value = value;

        /// <summary>
        /// Gets the email value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Email? email) => email?.Value ?? string.Empty;

        public static explicit operator Email(string email) => Create(email).Value();

        /// <summary>
        /// Creates a new <see cref="Email"/> instance based on the specified value.
        /// </summary>
        /// <param name="email">The email value.</param>
        /// <returns>The result of the email creation process containing the email or an error.</returns>
        public static Result<Email> Create(string? email)
        {
            IValidator<string> validator = new EmailNotNullOrEmptyValidator()
                .SetNext(new EmailMaxLengthValidator())
                .SetNext(new EmailFormatValidator());

            Result result = validator.Validate(email);

            if (result.IsFailure)
            {
                return Result.Failure<Email>(result.Error);
            }

            return Result.Success(new Email(email!));
        }

        /// <summary>
        /// Gets the empty email instance.
        /// </summary>
        internal static Email Empty => new Email(string.Empty);

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
