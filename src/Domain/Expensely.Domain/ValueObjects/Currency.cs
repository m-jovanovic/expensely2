using System.Collections.Generic;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.ValueObjects
{
    public class Currency : ValueObject
    {
        public static readonly Currency Usd = new Currency(1, "USD", "$");
        public static readonly Currency Eur = new Currency(2, "EUR", "€");
        public static readonly Currency Rsd = new Currency(3, "RSD", "din.");

        public static readonly IReadOnlyList<Currency> AllCurrencies = new List<Currency>
        {
            Usd,
            Eur,
            Rsd
        };

        private Currency(int id, string code, string sign)
        {
            Id = id;
            Sign = sign;
            Code = code;
        }

        public int Id { get; }

        public string Code { get; }

        public string Sign { get; }

        public static Currency? FromId(int currencyId)
            => ValueBetweenTwoValues(currencyId, 1, AllCurrencies.Count) ? AllCurrencies[currencyId - 1] : null;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Sign;
        }

        private static bool ValueBetweenTwoValues(int value, int fromInclusive, int toInclusive)
            => value >= fromInclusive && value <= toInclusive;
    }
}
