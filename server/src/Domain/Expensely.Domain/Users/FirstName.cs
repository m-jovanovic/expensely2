﻿using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;
using Expensely.Domain.Users.Validators.FirstName;

namespace Expensely.Domain.Users
{
    /// <summary>
    /// Represents the first name value object.
    /// </summary>
    public sealed class FirstName : ValueObject
    {
        /// <summary>
        /// The first name maximum length.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstName"/> class.
        /// </summary>
        /// <param name="value">The first name value.</param>
        private FirstName(string value) => Value = value;

        /// <summary>
        /// Gets the first name value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(FirstName? firstName) => firstName?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="firstName">The first name value.</param>
        /// <returns>The result of the first name creation process containing the first name or an error.</returns>
        public static Result<FirstName> Create(string? firstName)
        {
            IValidator<string> validator = new FirstNameNotNullOrEmptyValidator()
                .SetNext(new FirstNameMaxLengthValidator());

            Result result = validator.Validate(firstName);

            if (result.IsFailure)
            {
                return Result.Failure<FirstName>(result.Error);
            }

            return Result.Success(new FirstName(firstName!));
        }

        /// <summary>
        /// Gets the empty first name instance.
        /// </summary>
        internal static FirstName Empty => new FirstName(string.Empty);

        /// <summary>
        /// Gets the atomic values of the value object.
        /// </summary>
        /// <returns>The collection of objects representing the value object values.</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
