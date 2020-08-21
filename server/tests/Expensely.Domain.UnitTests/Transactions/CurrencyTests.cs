using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Transactions;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Transactions
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
        public void From_code_should_return_null_given_invalid_currency_id()
        {
            var currency = Currency.FromCode(string.Empty);

            currency.Should().BeNull();
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.0)]
        [InlineData(-1.0)]
        [InlineData(1213123123.123123123)]
        [InlineData(-1213123123.123123123)]
        public void Format_should_return_properly_formatted_string(decimal amount)
        {
            var currency = Currency.FromCode("USD")!;

            string formatted = currency.Format(amount);

            formatted.Should().Be($"{amount:n2} {currency.Code}");
        }

        private static IEnumerable<object[]> GetIdenticalCurrencyPairs()
        {
            return Currency.AllCurrencies().Select(currency => new object[] { currency, currency });
        }

        private static IEnumerable<object[]> GetDifferentCurrencyPairs()
        {
            IReadOnlyCollection<Currency> currencies = Currency.AllCurrencies();

            foreach (Currency currentCurrency in currencies)
            {
                foreach (var currency in currencies.Where(c => c != currentCurrency))
                {
                    yield return new object[] { currentCurrency, currency };
                }
            }
        }
    }
}
