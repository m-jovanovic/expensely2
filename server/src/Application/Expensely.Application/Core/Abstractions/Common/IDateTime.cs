using System;

namespace Expensely.Application.Core.Abstractions.Common
{
    /// <summary>
    /// Represents the interface for getting the current date and time.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current date and time in UTC format.
        /// </summary>
        DateTime UtcNow { get; }
    }
}