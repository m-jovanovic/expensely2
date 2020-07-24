using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Core.Primitives;
using FluentValidation.Results;

namespace Expensely.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when a validation fails.
    /// </summary>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="failures">The collection of validation failures.</param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation failures has occurred.")
        {
            ErrorCodes = failures
                .Select(failure => failure.ErrorCode)
                .Distinct()
                .Select(errorCode => new Error(errorCode))
                .ToList();
        }

        /// <summary>
        /// Gets the validation failures dictionary.
        /// </summary>
        public IReadOnlyList<Error> ErrorCodes { get; }
    }
}
