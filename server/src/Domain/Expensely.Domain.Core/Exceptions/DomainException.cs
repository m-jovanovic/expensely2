using System;

namespace Expensely.Domain.Core.Exceptions
{
    /// <summary>
    /// Represents the base domain exception.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="error">The error instance.</param>
        public DomainException(Error error) => Error = error;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }
    }
}
