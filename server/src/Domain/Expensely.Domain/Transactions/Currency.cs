using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Transactions
{
    /// <summary>
    /// Represents the currency value object.
    /// </summary>
    public sealed class Currency : ValueObject
    {
        public static readonly Currency None = new Currency(string.Empty, string.Empty, string.Empty);

        private static readonly Dictionary<string, Currency> Currencies = new Dictionary<string, Currency>
        {
            { "USD", new Currency("USD", "Dollar", "$") },
            { "EUR", new Currency("EUR", "Euro", "€") },
            { "RSD", new Currency("RSD", "Serbian dinar", "din.") }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <param name="code">The currency code.</param>
        /// <param name="name">The currency name.</param>
        /// <param name="symbol">The currency symbol.</param>
        private Currency(string code, string name, string symbol)
            : this()
        {
            Code = code;
            Name = name;
            Symbol = symbol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Currency()
        {
            Code = string.Empty;
            Name = string.Empty;
            Symbol = string.Empty;
        }

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the currency name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the currency symbol.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets the read-only collection of all currencies.
        /// </summary>
        /// <returns>The read-only collection of all currencies.</returns>
        public static IReadOnlyCollection<Currency> AllCurrencies() => Currencies.Values;

        /// <summary>
        /// Creates a new currency instance based on the specified currency identifier.
        /// </summary>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns>The currency instance with the specified identifier if it is found, otherwise null.</returns>
        public static Currency? FromCode(string currencyCode)
            => Currencies.TryGetValue(currencyCode, out Currency currency) ? currency : null;

        /// <summary>
        /// Formats the specified amount with the specified currency.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>The formatted string with the amount and currency code.</returns>
        public string Format(decimal amount) => $"{amount:n2} {Code}";

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
            yield return Symbol;
        }
    }
}
