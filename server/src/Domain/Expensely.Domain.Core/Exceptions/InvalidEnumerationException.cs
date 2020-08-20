using System;

namespace Expensely.Domain.Core.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when there is an attempt to create an invalid enumeration.
    /// </summary>
    public sealed class InvalidEnumerationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumerationException"/> class.
        /// </summary>
        public InvalidEnumerationException()
            : base("The specified type is not a valid enumeration type.")
        {
        }
    }
}
