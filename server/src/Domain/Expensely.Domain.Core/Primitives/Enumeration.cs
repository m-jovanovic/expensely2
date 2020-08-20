using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expensely.Domain.Core.Exceptions;

namespace Expensely.Domain.Core.Primitives
{
    /// <summary>
    /// Represents an enumeration type.
    /// </summary>
    public abstract class Enumeration : IEquatable<Enumeration>, IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected Enumeration()
        {
            Value = default;
            Name = string.Empty;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates an enumeration of the specified type based on the specified value.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The enumeration instance that matches the specified value.</returns>
        public static T FromValue<T>(int value)
            where T : Enumeration
        {
            T matchingItem = GetAll<T>().First(item => item.Value == value);

            return matchingItem;
        }

        /// <summary>
        /// Gets all of the enumeration values for the specified type.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The enumerable collection of values for the specified type.</returns>
        /// <exception cref="InvalidEnumerationException"> when <typeparamref name="T"/> is an invalid enumeration type.</exception>
        public static IEnumerable<T> GetAll<T>()
            where T : Enumeration
        {
            Type type = typeof(T);

            object? o = Activator.CreateInstance(type, true);

            if (o is null)
            {
                throw new InvalidEnumerationException();
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (FieldInfo info in fields)
            {
                var instance = (T)o;

                if (info.GetValue(instance) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public static bool operator ==(Enumeration a, Enumeration b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Enumeration a, Enumeration b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(Enumeration? other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() && other.Value.Equals(Value);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!(obj is Enumeration otherValue))
            {
                return false;
            }

            return GetType() == obj.GetType() && otherValue.Value.Equals(Value);
        }

        /// <inheritdoc />
        public int CompareTo(object? other)
        {
            return other is null ? 1 : Value.CompareTo(((Enumeration)other).Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}