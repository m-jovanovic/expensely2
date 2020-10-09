using System;

namespace Expensely.Domain.Core.Maybe
{
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T _value;

        private Maybe(T value) => _value = value;

        public bool HasValue => _value != null;

        public bool HasNoValue => !HasValue;

        public T Value
        {
            get
            {
                if (HasNoValue || _value is null)
                {
                    throw new InvalidOperationException("The value can not be accessed because it does not exist.");
                }

                return _value;
            }
        }

        public static Maybe<T> None => new Maybe<T>(default!);

        public static Maybe<T> From(T value) => new Maybe<T>(value);

        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);

        public bool Equals(Maybe<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (HasNoValue || other.HasNoValue)
            {
                return false;
            }

            return Value!.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is T value)
            {
                return Equals(new Maybe<T>(value));
            }

            if (obj is Maybe<T> maybe)
            {
                return Equals(maybe);
            }

            return false;
        }

        public override int GetHashCode() => HasNoValue ? 0 : Value!.GetHashCode();
    }
}
