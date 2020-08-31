﻿using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validation;
using Expensely.Domain.Users.Validators.LastName;

namespace Expensely.Domain.Users
{
    /// <summary>
    /// Represents the last name value object.
    /// </summary>
    public sealed class LastName : ValueObject
    {
        /// <summary>
        /// The last name maximum length.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastName"/> class.
        /// </summary>
        /// <param name="value">The last name value.</param>
        private LastName(string value) => Value = value;

        /// <summary>
        /// Gets the last name value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(LastName? lastName) => lastName?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="lastName">The last name value.</param>
        /// <returns>The result of the last name creation process containing the last name or an error.</returns>
        public static Result<LastName> Create(string? lastName)
        {
            IValidator<string> validator = new LastNameNotNullOrEmptyValidator()
                .SetNext(new LastNameMaxLengthValidator());

            Result result = validator.Validate(lastName);

            if (result.IsFailure)
            {
                return Result.Failure<LastName>(result.Error);
            }

            return Result.Success(new LastName(lastName!));
        }

        /// <summary>
        /// Gets the empty last name instance.
        /// </summary>
        internal static LastName Empty => new LastName(string.Empty);

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