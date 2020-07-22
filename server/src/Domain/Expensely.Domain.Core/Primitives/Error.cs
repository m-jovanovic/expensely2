using System.Collections.Generic;

namespace Expensely.Domain.Core.Primitives
{
    /// <summary>
    /// Represents a concrete application error.
    /// </summary>
    public sealed class Error : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        public Error(string code) => Code = code;

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }

        /// <summary>
        /// Gets the empty error instance.
        /// </summary>
        internal static Error None => new Error(string.Empty);
    }
}
