using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class CurrencyTests
    {
        [Theory]
        [MemberData(nameof(GetIdenticalCurrencyPairs))]
        public void Should_be_equal_given_same_currencies(Currency currency1, Currency currency2)
        {
            Assert.Equal(currency1, currency2);
            Assert.Equal(currency2, currency1);
            Assert.True(currency1 == currency2);
            Assert.True(currency2 == currency1);
            Assert.Equal(currency1.GetHashCode(), currency2.GetHashCode());
            Assert.Equal(currency2.GetHashCode(), currency1.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(GetDifferentCurrencyPairs))]
        public void Should_not_be_equal_given_different_currencies(Currency currency1, Currency currency2)
        {
            Assert.NotEqual(currency1, currency2);
            Assert.NotEqual(currency2, currency1);
            Assert.True(currency1 != currency2);
            Assert.True(currency2 != currency1);
            Assert.NotEqual(currency1.GetHashCode(), currency2.GetHashCode());
            Assert.NotEqual(currency2.GetHashCode(), currency1.GetHashCode());
        }

        [Fact]
        public void FromId_should_return_null_given_invalid_currency_id()
        {
            var currency = Currency.FromId(default);

            Assert.Null(currency);
        }

        [Fact]
        public void FromId_should_return_null_given_out_of_bounds_currency_id()
        {
            int currencyId = Currency.AllCurrencies.Count + 1;

            var currency = Currency.FromId(currencyId);

            Assert.Null(currency);
        }

        [Fact]
        public void FromId_should_return_currency_give_min_currency_id()
        {
            int currencyId = 1;

            var currency = Currency.FromId(currencyId);

            Assert.NotNull(currency);
        }

        [Fact]
        public void FromId_should_return_currency_given_max_currency_id()
        {
            int currencyId = Currency.AllCurrencies.Count;

            var currency = Currency.FromId(currencyId);

            Assert.NotNull(currency);
        }

        private static IEnumerable<object[]> GetIdenticalCurrencyPairs()
        {
            return Currency.AllCurrencies.Select(currency => new object[] { currency, currency });
        }

        private static IEnumerable<object[]> GetDifferentCurrencyPairs()
        {
            foreach (Currency currentCurrency in Currency.AllCurrencies)
            {
                foreach (var currency in Currency.AllCurrencies.Where(c => c != currentCurrency))
                {
                    yield return new object[] { currentCurrency, currency };
                }
            }
        }
    }
}
