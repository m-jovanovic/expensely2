using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class CurrencyTests
    {
        [Theory]
        [MemberData(nameof(GetIdenticalCurrencyPairs))]
        public void Should_be_equal_given_same_currencies(Currency currency1, Currency currency2)
        {
            currency1.Should().Be(currency2);
            currency2.Should().Be(currency1);
            (currency1 == currency2).Should().BeTrue();
            (currency2 == currency1).Should().BeTrue();
            currency1.GetHashCode().Should().Be(currency2.GetHashCode());
            currency2.GetHashCode().Should().Be(currency1.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(GetDifferentCurrencyPairs))]
        public void Should_not_be_equal_given_different_currencies(Currency currency1, Currency currency2)
        {
            currency1.Should().NotBe(currency2);
            currency2.Should().NotBe(currency1);
            (currency1 != currency2).Should().BeTrue();
            (currency2 != currency1).Should().BeTrue();
            currency1.GetHashCode().Should().NotBe(currency2.GetHashCode());
            currency2.GetHashCode().Should().NotBe(currency1.GetHashCode());
        }

        [Fact]
        public void FromId_should_return_null_given_invalid_currency_id()
        {
            var currency = Currency.FromId(default);

            currency.Should().BeNull();
        }

        [Fact]
        public void FromId_should_return_null_given_out_of_bounds_currency_id()
        {
            int currencyId = Currency.AllCurrencies.Count + 1;

            var currency = Currency.FromId(currencyId);

            currency.Should().BeNull();
        }

        [Fact]
        public void FromId_should_return_currency_give_min_currency_id()
        {
            int currencyId = 1;

            var currency = Currency.FromId(currencyId);

            currency.Should().NotBeNull();
        }

        [Fact]
        public void FromId_should_return_currency_given_max_currency_id()
        {
            int currencyId = Currency.AllCurrencies.Count;

            var currency = Currency.FromId(currencyId);

            currency.Should().NotBeNull();
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
