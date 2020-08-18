using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.ValueObjects
{
    /// <summary>
    /// Represents the currency value object.
    /// </summary>
    public sealed class Currency : ValueObject
    {
        public static readonly Currency None = new Currency(0, string.Empty, string.Empty);
        public static readonly Currency Usd = new Currency(1, "USD", "$");
        public static readonly Currency Eur = new Currency(2, "EUR", "€");
        public static readonly Currency Rsd = new Currency(3, "RSD", "din.");

        public static readonly IReadOnlyList<Currency> AllCurrencies = new List<Currency>
        {
            Usd,
            Eur,
            Rsd
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="id">The currency identifier.</param>
        /// <param name="code">The currency code.</param>
        /// <param name="sign">The currency sign.</param>
        private Currency(int id, string code, string sign)
        {
            Id = id;
            Sign = sign;
            Code = code;
        }

        /// <summary>
        /// Gets the currency identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the currency sign.
        /// </summary>
        public string Sign { get; }

        /// <summary>
        /// Creates a new currency instance based on the specified currency identifier.
        /// </summary>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns>The currency instance with the specified identifier if it is found, otherwise null.</returns>
        public static Currency? FromId(int currencyId)
            => ValueBetweenTwoValues(currencyId, 1, AllCurrencies.Count) ? AllCurrencies[currencyId - 1] : null;

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Sign;
        }

        /// <summary>
        /// Determines if the specified value is between the lower and upper inclusive bounds.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="fromInclusive">The lower inclusive bound.</param>
        /// <param name="toInclusive">The upper inclusive bound.</param>
        /// <returns>True if the specified value is within the specified lower and upper bound, otherwise false.</returns>
        private static bool ValueBetweenTwoValues(int value, int fromInclusive, int toInclusive)
            => value >= fromInclusive && value <= toInclusive;
    }
}
