﻿using System;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Utility
{
    /// <summary>
    /// Contains assertions for the most common application checks.
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Ensures that the specified <see cref="Guid"/> value is not empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The message to show if the check fails.</param>
        /// <param name="argumentName">The name of the argument being checked.</param>
        /// <exception cref="ArgumentException"> if the specified value is empty.</exception>
        public static void NotEmpty(Guid value, string message, string argumentName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>
        /// Ensures that the specified <see cref="string"/> value is not empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The message to show if the check fails.</param>
        /// <param name="argumentName">The name of the argument being checked.</param>
        /// <exception cref="ArgumentException"> if the specified value is empty.</exception>
        public static void NotEmpty(string value, string message, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>
        /// Ensures that the specified <see cref="DateTime"/> value is not empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The message to show if the check fails.</param>
        /// <param name="argumentName">The name of the argument being checked.</param>
        /// <exception cref="ArgumentException"> if the specified value is null.</exception>
        public static void NotEmpty(DateTime value, string message, string argumentName)
        {
            if (value == default)
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>
        /// Ensures that the specified <see cref="Money"/> value is not empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The message to show if the check fails.</param>
        /// <param name="argumentName">The name of the argument being checked.</param>
        /// <exception cref="ArgumentException"> if the specified value is null.</exception>
        public static void NotEmpty(Money value, string message, string argumentName)
        {
            if (value == Money.None)
            {
                throw new ArgumentException(message, argumentName);
            }
        }
    }
}
